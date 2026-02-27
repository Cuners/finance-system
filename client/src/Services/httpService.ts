type HttpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH';

interface RequestConfig {
  method?: HttpMethod;
  headers?: HeadersInit;
  body?: BodyInit | null;
}

// Основная функция запроса
async function request<T>(
  url: string,
  config: RequestConfig = {}
): Promise<T> {
  const { method = 'GET', headers = {}, body = null } = config;

  // Базовые заголовки
  const defaultHeaders: HeadersInit = {
    'Content-Type': 'application/json',
    ...headers
  };

  const response = await fetch(url, {
    method,
    headers: defaultHeaders,
    body
  });

  // Глобальная обработка ошибок
  if (!response.ok) {
    const errorText = await response.text();
    try {
      // Пытаемся распарсить JSON-ошибку
      const errorJson = JSON.parse(errorText);
      throw new Error(errorJson.message || 'Unknown error');
    } catch {
      // Если не JSON — возвращаем текст
      throw new Error(`HTTP ${response.status}: ${errorText}`);
    }
  }
  const contentType = response.headers.get('content-type');
  if (!contentType || !contentType.includes('application/json')) {
    throw new Error(`Expected JSON, but got ${contentType || 'no content-type'}`);
  }
  // Парсим JSON, если есть контент
  if (response.headers.get('content-length') === '0') {
    return undefined as T;
  }

  try {
    return await response.json();
  } catch (error) {
    throw new Error('Failed to parse JSON response');
  }
}

// Экспорт методов
export const httpService = {
  get: <T>(url: string, headers?: HeadersInit) => 
    request<T>(url, { method: 'GET', headers }),

  post: <T>(url: string, data: any, headers?: HeadersInit) => 
    request<T>(url, { method: 'POST', headers, body: JSON.stringify(data) }),

  put: <T>(url: string, data: any, headers?: HeadersInit) => 
    request<T>(url, { method: 'PUT', headers, body: JSON.stringify(data) }),

  delete: <T>(url: string, headers?: HeadersInit) => 
    request<T>(url, { method: 'DELETE', headers }),

  patch: <T>(url: string, data: any, headers?: HeadersInit) => 
    request<T>(url, { method: 'PATCH', headers, body: JSON.stringify(data) })
};