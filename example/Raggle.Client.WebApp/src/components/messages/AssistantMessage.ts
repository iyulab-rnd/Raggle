import { LitElement, css, html, nothing } from "lit";
import { customElement, property } from "lit/decorators.js";

import type { Message, MessageContent } from "../../models";
import { clone } from "../../services";

@customElement('assistant-message')
export class AssistantMessage extends LitElement {

  @property({ type: Object })
  message!: Message;

  render() {
    if (!this.message) return nothing;

    return html`
      <div class="container">
        <img class="avatar"
          src="/assets/images/bot-avatar.png" />
        <div class="name">
          ${this.message?.name || 'Assistant'}
        </div>
        <div class="content">
          ${this.message?.content?.map(item => {
            if (item.type === 'text') {
              return html`
                <markdown-block
                  .content=${item.text || ''}
                ></markdown-block>
              `;
            } else if (item.type === 'image') {
              return html`
                <image-block
                  data=${item.data || ''}
                ></image-block>
              `;
            } else if (item.type === 'tool') {
              return html`
                <tool-block
                  .name=${item.name || ''}
                  .result=${item.result || ''}
                ></tool-block>
              `;
            } else {
              return nothing;
            }
          })}
        </div>
      </div>
    `;
  }

  public appendContent = (content: MessageContent) => {
    this.message.content ??= [];
    const item = this.message.content.find(i => i.index === content.index);
    
    if (!item) {
      const _content = clone(content);
      this.message.content.push(_content);
    } else if (item.type === 'text' && content.type === 'text') {
      item.text ??= '';
      item.text += content.text;
    } else if (item.type === 'image' && content.type === 'image') {
      item.data = content.data;
    } else if (item.type === 'tool' && content.type === 'tool') {
      item.id = content.id;
      item.name = content.name;
      item.arguments = content.arguments;
      item.result = content.result;
    } else {
      throw new Error('TODO: Implement appendContent for other types');
    }
    this.requestUpdate();
  }

  static styles = css`
    :host {
      width: 100%;
      display: block;
    }

    .container {
      display: grid;
      grid-template-areas: 
        "avatar name"
        "avatar content";
      grid-template-columns: auto 1fr;
      grid-template-rows: auto 1fr;
      gap: 12px;
    }

    .avatar {
      grid-area: avatar;
      width: 40px;
      height: 40px;
    }

    .name {
      width: 100%;
      grid-area: name;
      font-size: 12px;
      font-weight: 600;
      color: var(--sl-color-gray-600);
    }

    .content {
      width: 100%;
      grid-area: content;
      display: flex;
      flex-direction: column;
      gap: 4px;
      border-radius: 4px;
      padding: 8px;
      background-color: var(--sl-color-gray-100);
      border: 1px solid var(--sl-color-gray-200);
      box-sizing: border-box;
    }
  `;
}
