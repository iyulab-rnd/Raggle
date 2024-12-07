import { LitElement, css, html } from "lit";
import { customElement, state } from "lit/decorators.js";
import type { Collection } from "../backend/Models";
import { API } from "../backend/ApiClient";

@customElement('memory-page')
export class MemoryPage extends LitElement {

  @state() mode: 'view' | 'edit' | 'none' = 'none';

  @state() collections?: Collection[] = [];
  @state() collection?: Collection;

  connectedCallback(): void {
    super.connectedCallback();
    this.loadCollections();
  }

  render() {
    return html`
      <main-layout ratio="1:2:2">
        <main-list
          create-label="New Collection +"
          slot="left"
          key="collectionId"
          .items=${this.collections || []}
          @create=${this.createCollection}
          @delete=${this.deleteCollection}
          @select=${this.selectedCollection}
        ></main-list>
        <div slot="main">
          ${this.mode === 'view' ? html`
            <storage-view
              .collection=${this.collection}
            ></storage-view>
          ` 
          :this.mode === 'edit' ? html`
            <collection-form
              @submit=${this.loadCollections}
            ></collection-form>
          `
          : html`None`}
        </div>
        <div slot="right">
          
        </div>
      </main-layout>
    `;
  }

  private async loadCollections() {
    this.collections = await API.findCollectionsAsync();
  }

  private createCollection() {
    this.mode = 'edit';
  }

  private selectedCollection(event: CustomEvent) {
    const key = event.detail;
    this.collection = this.collections?.find(c => c.collectionId === key);
    this.mode = 'view';
  }

  private async deleteCollection(event: CustomEvent) {
    console.log(event.detail);
    await API.deleteCollectionAsync(event.detail);
    await this.loadCollections();
  }

  static styles = css`
    :host {
      display: block;
      width: 100%;
      height: 100%;
    }
  `;
}
