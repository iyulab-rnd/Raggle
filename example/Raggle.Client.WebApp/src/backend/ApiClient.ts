import axios, { AxiosInstance } from 'axios';
import { 
  AssistantEntity, 
  CollectionEntity, 
  Message, 
  ServiceModels, 
  StreamingChatResponse
} from '../models';

export class API {
  private static readonly _baseURL: string = import.meta.env.DEV
    ? import.meta.env.VITE_API || 'http://localhost:5000/v1'
    : window.location.origin + '/v1';
  private static readonly _client: AxiosInstance = axios.create({
    baseURL: this._baseURL,
    headers: {
      'Content-Type': 'application/json',
    },
  });

  // Model API

  public static async getChatModelsAsync(): Promise<ServiceModels> {
    try {
      const res = await this._client.get<ServiceModels>('/models/chat');
      return res.data;
    } catch (error) {
      console.error('Error fetching chat models:', error);
      throw error;
    }
  }

  public static async getEmbeddingModelsAsync(): Promise<ServiceModels> {
    try {
      const res = await this._client.get<ServiceModels>('/models/embedding');
      return res.data;
    } catch (error) {
      console.error('Error fetching embedding models:', error);
      throw error;
    }
  }

  // Assistant API

  public static async getAssistantsAsync(skip: number = 0, limit: number = 20): Promise<AssistantEntity[]> {
    try {
      const res = await this._client.get<AssistantEntity[]>('/assistants', {
        params: { skip, limit },
      });
      return res.data;
    } catch (error) {
      console.error('Error fetching assistants:', error);
      throw error;
    }
  }

  public static async getAssistantAsync(assistantId: string): Promise<AssistantEntity> {
    try {
      const res = await this._client.get<AssistantEntity>(`/assistants/${assistantId}`);
      return res.data;
    } catch (error) {
      console.error('Error fetching assistant:', error);
      throw error;
    }
  }

  public static async upsertAssistantAsync(assistant: AssistantEntity): Promise<AssistantEntity> {
    try {
      const res = await this._client.post<AssistantEntity>('/assistants', assistant);
      return res.data;
    } catch (error) {
      console.error('Error upserting assistant:', error);
      throw error;
    }
  }

  public static async deleteAssistantAsync(assistantId: string): Promise<void> {
    try {
      await this._client.delete(`/assistants/${assistantId}`);
    } catch (error) {
      console.error('Error deleting assistant:', error);
      throw error;
    }
  }

  public static chatAssistantAsync(
    assistantId: string,
    messages: Message[],
    on: (message: StreamingChatResponse) => void
  ): AbortController {
    const controller = new AbortController();
    const { signal } = controller;
  
    (async () => {
      try {
        console.log('Sending messages:', assistantId, messages);
  
        const response = await fetch(`${this._baseURL}/assistants/${assistantId}/chat`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(messages),
          signal,
        });
  
        if (!response.ok || !response.body) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
  
        const reader = response.body.getReader();
        const decoder = new TextDecoder('utf-8');
        let buffer = '';
  
        while (true) {
          const { done, value } = await reader.read();
          if (done) break;
  
          buffer += decoder.decode(value, { stream: true });
          let lines = buffer.split('\n');
          buffer = lines.pop() || '';
  
          for (const line of lines) {
            if (line.trim()) { // 빈 줄 무시
              try {
                const parsed = JSON.parse(line);
                console.log('Received:', parsed);
                on(parsed);
              } catch (e) {
                console.error('Error parsing JSON:', e);
              }
            }
          }
        }
  
        // 남아있는 버퍼 처리
        if (buffer.trim()) {
          try {
            const parsed = JSON.parse(buffer);
            on(parsed);
          } catch (e) {
            console.error('Error parsing JSON:', e);
          }
        }
  
      } catch (error: any) {
        if (error.name === 'AbortError') {
          console.log('Stream aborted');
        } else {
          console.error('Fetch error:', error);
        }
      }
    })(); // IIFE를 사용하여 비동기 작업 실행
  
    return controller; // AbortController를 즉시 반환
  }

  // Memory API
  
  public static async findCollectionsAsync(
    name?: string,
    limit: number = 10,
    skip: number = 0,
    order: string = 'desc'
  ): Promise<CollectionEntity[]> {
    try {
      const response = await this._client.get<CollectionEntity[]>('/memory', { 
        params: { limit, skip, order, name }
      });
      return response.data;
    } catch (error) {
      console.error('Error finding collections:', error);
      throw error;
    }
  }

  public static async upsertCollectionAsync(collection: CollectionEntity): Promise<CollectionEntity> {
    try {
      const response = await this._client.post<CollectionEntity>('/memory', collection);
      return response.data;
    } catch (error) {
      console.error('Error upserting collection:', error);
      throw error;
    }
  }

  public static async deleteCollectionAsync(collectionId: string): Promise<void> {
    try {
      await this._client.delete(`/memory/${collectionId}`);
    } catch (error) {
      console.error('Error deleting collection:', error);
      throw error;
    }
  }

  public static async searchCollectionAsync(collectionId: string, query: string): Promise<any> {
    try {
      const response = await this._client.post(`/memory/${collectionId}/search`, {
        query,
      });
      return response;
    } catch (error) {
      console.error('Error searching document:', error);
      throw error;
    }
  }

  public static async findDocumentsAsync(
    collectionId: string,
    name?: string,
    limit: number = 10,
    skip: number = 0,
    order: string = 'desc'
  ): Promise<Document[]> {
    try {
      const params: any = { limit, skip, order };
      if (name) {
        params.name = name;
      }

      const response = await this._client.get<Document[]>(`/memory/${collectionId}/documents`,
        { params }
      );
      return response.data;
    } catch (error) {
      console.error('문서 조회 실패:', error);
      throw error;
    }
  }

  public static async uploadDocumentAsync(collectionId: string, file: File): Promise<void> {
    const formData = new FormData();
    formData.append("file", file);
    try {
      await this._client.post(`/memory/${collectionId}/documents`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
        onUploadProgress(progressEvent) {
          console.log('Upload progress:', progressEvent.progress);
        },
      });
    } catch (error) {
      console.error('File upload failed:', error);
    }
  }

  public static async deleteDocumentAsync(collectionId: string, documentId: string): Promise<void> {
    try {
      await this._client.delete(`/memory/${collectionId}/documents/${documentId}`);
      console.log(`문서 ${documentId} 삭제 완료`);
    } catch (error) {
      console.error('문서 삭제 실패:', error);
    }
  }
  
}
