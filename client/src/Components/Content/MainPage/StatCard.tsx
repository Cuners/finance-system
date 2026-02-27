import "./StatCard.css"
import BalanceIcon from '../../../assets/SvgIcons/BalanceIcon';
import ExpenseIcon from '../../../assets/SvgIcons/ExpenseIcon';
import IncomeIcon from '../../../assets/SvgIcons/IncomeIcon';
import SavingIcon from '../../../assets/SvgIcons/SavingIcon';
export type StatCardVariant = 'balance' | 'income' | 'expense' | 'saving';

interface StatCardProps {
  title: string;
  value: string;
  changeValue: number; 
  variant: StatCardVariant;
}

const StatCard = ({
  title,
  value,
  changeValue,
  variant
}:StatCardProps) => {
    const getIcon = () => {
    switch (variant) {
      case 'balance': return <BalanceIcon />;
      case 'income': return <IncomeIcon />;
      case 'expense': return <ExpenseIcon />;
      case 'saving': return <SavingIcon />;
      default: return <BalanceIcon />;
    }
  };
  const isPositive = changeValue >= 0;
  const formattedChange = `${isPositive ? '+' : ''}${changeValue.toFixed(1)}% vs last month`;
  const TrendIcon = isPositive ? IncomeIcon : ExpenseIcon;
  return (
    <div className={`stat-card ${variant}`}>
      <div>
        <div className="stat-header">
          <span className="stat-title">{title}</span>
        </div>
        <div className="stat-value">{value}</div>
        <div className="stat-change">
        <span className={isPositive ? 'up' : 'down'}>
          <TrendIcon />
          {formattedChange}
        </span>
        </div>
      </div>
      <div className="stat-icon">{getIcon()}</div>
    </div>
  );
};

export default StatCard;