/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly BASE_URL: string;
  readonly VITE_API: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
