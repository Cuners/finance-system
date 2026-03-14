type HttpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH';

interface RequestConfig {
  method?: HttpMethod;
  headers?: HeadersInit;
  body?: BodyInit | null;
  credentials?: RequestCredentials; 
}

async function request<T>(
  url: string,
  config: RequestConfig = {}
): Promise<T> {
  const { 
    method = 'GET', 
    headers = {}, 
    body = null,
    credentials = 'include' 
  } = config;

  const defaultHeaders: HeadersInit = {
    'Content-Type': 'application/json',
    ...headers
  };

  const response = await fetch(url, {
    method,
    headers: defaultHeaders,
    body,
    credentials,
  });

  if (!response.ok) {
    const errorText = await response.text();
    try {
      const errorJson = JSON.parse(errorText);
      throw new Error(errorJson.message || 'Unknown error');
    } catch {
      throw new Error(`HTTP ${response.status}: ${errorText}`);
    }
  }

  if (response.status === 204) {
    return undefined as T;
  }

  const contentType = response.headers.get('content-type');
  if (!contentType || !contentType.includes('application/json')) {
    return undefined as T;
  }

  if (response.headers.get('content-length') === '0') {
    return undefined as T;
  }

  try {
    return await response.json();
  } catch (error) {
    throw new Error('Failed to parse JSON response');
  }
}

export const httpService = {
  get: <T>(url: string, headers?: HeadersInit) => 
    request<T>(url, { method: 'GET', headers }),

  post: <T>(url: string, data: unknown, headers?: HeadersInit) => 
    request<T>(url, { 
      method: 'POST', 
      headers, 
      body: JSON.stringify(data) 
    }),

  put: <T>(url: string, data: unknown, headers?: HeadersInit) => 
    request<T>(url, { 
      method: 'PUT', 
      headers, 
      body: JSON.stringify(data) 
    }),

  delete: <T>(url: string, headers?: HeadersInit) => 
    request<T>(url, { method: 'DELETE', headers }),

  patch: <T>(url: string, data: unknown, headers?: HeadersInit) => 
    request<T>(url, { 
      method: 'PATCH', 
      headers, 
      body: JSON.stringify(data) 
    }),
};