type HttpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH';

interface RequestConfig {
  method?: HttpMethod;
  headers?: HeadersInit;
  body?: BodyInit | null;
  credentials?: RequestCredentials;
  skipAuthRetry?: boolean; // Флаг: пропустить обработку 401
}

// Глобальное состояние для управления обновлением токена
let isRefreshing = false;
type QueuedCallback = (success: boolean) => void;
let requestQueue: QueuedCallback[] = [];

/**
 * Обновляет access token через эндпоинт /auth/refresh
 * true, если обновление успешно, false — если ошибка
 */
const refreshAccessToken = async (): Promise<boolean> => {
  try {
    console.log('Refreshing access token...');
    
    const response = await fetch('/api/auth/Auth/refresh', {
      method: 'POST',
      credentials: 'include',
      headers: { 'Content-Type': 'application/json' }
    });

    if (response.ok) {
      console.log('Access token refreshed successfully');
      return true;
    }

    // refreshToken истёк
    if (response.status === 401) {
      console.warn('Refresh token expired, redirecting to login');
      window.location.href = '/login';
      return false;
    }

    console.error('Failed to refresh token:', response.status);
    return false;
  } catch (error) {
    console.error('Error during token refresh:', error);
    return false;
  }
};

/**
 * Выполняет все запросы из очереди после обновления токена
 */
const processQueue = (success: boolean) => {
  requestQueue.forEach(callback => callback(success));
  requestQueue = [];
};

async function request<T>(
  url: string,
  config: RequestConfig = {}
): Promise<T> {
  const { 
    method = 'GET', 
    headers = {}, 
    body = null,
    credentials = 'include', 
    skipAuthRetry = false    // По умолчанию обрабатываем 401
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

  // Токен истёк
  if (response.status === 401 && !skipAuthRetry) {
    
    // Если обновление уже идёт — добавляем запрос в очередь
    if (isRefreshing) {
      console.log('Token refresh in progress, queuing request:', url);
      
      return new Promise((resolve, reject) => {
        requestQueue.push((success: boolean) => {
          if (success) {
            // Повторяем исходный запрос с флагом skipAuthRetry
            request<T>(url, { ...config, skipAuthRetry: true })
              .then(resolve)
              .catch(reject);
          } else {
            reject(new Error('Token refresh failed'));
          }
        });
      });
    }

    // Запускаем обновление токена
    console.log('401 received, starting token refresh...');
    isRefreshing = true;
    
    const success = await refreshAccessToken();
    isRefreshing = false;

    if (success) {
      // Обновление успешно
      console.log('Processing queued requests...');
      processQueue(true);
      
      return request<T>(url, { ...config, skipAuthRetry: true });
    } else {
      // Обновление не удалось
      console.error('Token refresh failed, rejecting queued requests');
      processQueue(false);
      throw new Error('Authentication failed: please log in again');
    }
  }

  // Стандартная обработка ошибок
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