// types/http.ts
export type HttpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH';

export interface RequestConfig {
  method?: HttpMethod;
  headers?: HeadersInit;
  body?: unknown; //  Любые данные, сериализуем
  credentials?: RequestCredentials; //  include , same-origin, omit
  signal?: AbortSignal; //  Для отмены запроса
}

export interface ApiService {
  get<T>(url: string, config?: Omit<RequestConfig, 'method' | 'body'>): Promise<T>;
  post<T>(url: string, data?: unknown, config?: Omit<RequestConfig, 'method'>): Promise<T>;
  put<T>(url: string, data?: unknown, config?: Omit<RequestConfig, 'method'>): Promise<T>;
  delete<T>(url: string, config?: Omit<RequestConfig, 'method' | 'body'>): Promise<T>;
  patch<T>(url: string, data?: unknown, config?: Omit<RequestConfig, 'method'>): Promise<T>;
}