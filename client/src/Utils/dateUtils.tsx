export const getDateRangeForPeriod = (period: 'this-month' | 'last-month' | 'this-year') => {
  const now = new Date();
  const year = now.getFullYear();
  const month = now.getMonth();

  switch (period) {
    case 'this-month':
      return {
        startDate: new Date(year, month, 1),
        endDate: new Date(year, month + 1, 0, 23, 59, 59, 999)
      };
    
    case 'last-month':
      const prevMonth = month === 0 ? 11 : month - 1;
      const prevYear = month === 0 ? year - 1 : year;
      
      return {
        startDate: new Date(prevYear, prevMonth, 1),
        endDate: new Date(prevYear, prevMonth + 1, 0, 23, 59, 59, 999)
      };
    
    case 'this-year':
      return {
        startDate: new Date(year, 0, 1),
        endDate: new Date(year, 11, 31, 23, 59, 59, 999)
      };
    
    default:
      return {
        startDate: new Date(0),
        endDate: new Date()
      };
  }
};