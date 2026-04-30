
/**
 * Форматирует числовое значение как валюту RUB
 * @param value - числовое значение для форматирования
 * @param options - дополнительные опции форматирования
 * @returns отформатированная строка валюты
 */
export const formatCurrency = (
  value: number, 
  options?: {
    minimumFractionDigits?: number;
    maximumFractionDigits?: number;
  }
): string => {
  return new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB',
    minimumFractionDigits: options?.minimumFractionDigits ?? 2,
    maximumFractionDigits: options?.maximumFractionDigits ?? 2,
  }).format(value);
};

/**
 * Форматирует дату в локализованный месяц
 * @param date - объект Date для форматирования (по умолчанию текущая дата)
 * @returns отформатированное название месяца
 */
export const formatMonth = (date: Date = new Date()): string => {
  const formatter = new Intl.DateTimeFormat('ru-RU', { month: 'long' });
  return formatter.format(date);
};

/**
 * Форматирует дату в локализованный формат
 * @param date - строка даты или объект Date
 * @param options - опции форматирования даты
 * @returns отформатированная строка даты
 */
export const formatDate = (
  date: string | Date, 
  options?: Intl.DateTimeFormatOptions
): string => {
  const dateObj = typeof date === 'string' ? new Date(date) : date;
  return dateObj.toLocaleDateString('ru-RU', options);
};

/**
 * Форматирует числовое значение с разделителями тысяч
 * @param value - числовое значение
 * @returns отформатированная строка с разделителями
 */
export const formatNumber = (value: number): string => {
  return value.toLocaleString('ru-RU', { minimumFractionDigits: 2 });
};

/**
 * Форматирует процентное значение
 * @param value - процентное значение
 * @param decimals - количество знаков после запятой (по умолчанию 1)
 * @returns отформатированная строка процента
 */
export const formatPercent = (value: number, decimals: number = 1): string => {
  return `${Math.min(100, value).toFixed(decimals)}%`;
};

/**
 * Форматирует изменение в процентах со знаком
 * @param value - процентное изменение
 * @param decimals - количество знаков после запятой (по умолчанию 1)
 * @returns отформатированная строка изменения
 */
export const formatPercentChange = (value: number, decimals: number = 1): string => {
  const sign = value >= 0 ? '+' : '';
  return `${sign}${value.toFixed(decimals)}%`;
};

export const getLocalDateString = (value: Date) => {
  const now = value;
  const year = now.getFullYear();
  const month = String(now.getMonth() + 1).padStart(2, '0');
  const day = String(now.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
};

/**
 * Получает знак валюты для RUB
 * @returns символ валюты RUB
 */
export const getCurrencySymbol = (): string => {
  const formatter = new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB',
    currencyDisplay: 'symbol'
  });
  return formatter.format(0).replace(/[\d,.\s]+/g, '').trim();
};