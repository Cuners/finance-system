import "./StatCardBudget.css"
import BalanceIcon from '../../../assets/SvgIcons/BalanceIcon';
import ExpenseIcon from '../../../assets/SvgIcons/ExpenseIcon';
import SavingIcon from '../../../assets/SvgIcons/SavingIcon';
export type StatCardVariant = 'balance' | 'expense' | 'saving';

interface StatCardProps {
  title: string;
  value: string;
  variant: StatCardVariant;
}

const StatCardBudget = ({
  title,
  value,
  variant
}:StatCardProps) => {
    const getIcon = () => {
    switch (variant) {
      case 'balance': return <BalanceIcon />;
      case 'expense': return <ExpenseIcon />;
      case 'saving': return <SavingIcon />;
      default: return <BalanceIcon />;
    }
  };
    const getSubTitle = () => {
    switch (variant) {
      case 'balance': return "Monthly Allocation";
      case 'expense': return "Budget used";
      case 'saving': return "Available to spend";
      default: return "Monthly allocation";
    }
  };
  return (
    <div className={`stat-card ${variant}`}>
      <div>
        <div className="stat-header">
          <span className="stat-title">{title}</span>
        </div>
        <div className="stat-value">{value}</div>
        <div className="stat-change">
        <span >
            {getSubTitle()}
        </span>
        </div>
      </div>
      <div className="stat-icon">{getIcon()}</div>
    </div>
  );
};

export default StatCardBudget;