// src/utils/categoryIcons.ts
import FoodIcon from '../assets/SvgIcons/FoodIcon';
import TransportIcon from '../assets/SvgIcons/TransportIcon';
import EntertainmentIcon from '../assets/SvgIcons/EntertainmentIcon';
import HealthIcon from '../assets/SvgIcons/HealthIcon';
import UtilitiesIcon from '../assets/SvgIcons/UtilitiesIcon';
import ShoppingIcon from '../assets/SvgIcons/ShoppingIcon';
import TravelIcon from '../assets/SvgIcons/TravelIcon';
import EducationIcon from '../assets/SvgIcons/EducationIcon';

export const getCategoryIcon = (categoryName: string) => {
  const name = categoryName.toLowerCase().trim();
  
  switch (name) {

    case 'продукты':
      return <FoodIcon />;

    case 'транспорт':
      return <TransportIcon />;

    case 'развлечения':
      return <EntertainmentIcon />;

    case 'здоровье':
      return <HealthIcon />;

    case 'покупки':
      return <ShoppingIcon />;

    case 'путешествия':
      return <TravelIcon />;
    
    case 'образование':
      return <EducationIcon />;

    default:
      return <UtilitiesIcon />;
  }
};

export const getCategoryColor = (categoryName: string) => {
  const name = categoryName.toLowerCase().trim();
  
  switch (name) {
    case 'продукты':
      return '#f59e0b'; 
    
    case 'транспорт':
      return '#3b82f6'; 
    
    case 'развлечения':
      return '#8b5cf6'; 

    case 'здоровье':
      return '#10b981'; 
    
    case 'коммунальные услуги':
      return '#6b7280'; 
    
    case 'покупки':
      return '#ec4899';
    
    case 'путешествия':
      return '#f43f5e';
    
    case 'образование':
      return '#0ea5e9';
    
    default:
      return '#6b7280'; 
  }
};

export interface CategoryInfo {
  icon: React.ReactNode;
  color: string;
}

export const getCategoryInfo = (categoryName: string): CategoryInfo => {
  return {
    icon: getCategoryIcon(categoryName),
    color: getCategoryColor(categoryName)
  };
};