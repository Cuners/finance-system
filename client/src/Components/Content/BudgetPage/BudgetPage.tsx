import StatCardBudget from "./StatCardBudget";
import "./BudgetPage.css";
import {useBudgetsSummary} from '../../../Hooks/useBudgetsSummary';
import BudgetCategories from "./BudgetCategories";
import TransactionsHeader from "../ContentHeader.tsx";
const BudgetPage = () => {
    const { budgets, loading, error } = useBudgetsSummary();
    const formatCurrency = (value: number) => 
    new Intl.NumberFormat('ru-RU', {style: 'currency',currency: 'RUB'}).format(value);

    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;
    return(
      <div className="budget-page">
        <TransactionsHeader
          title="Бюджеты"
          description="Планируйте и отслеживайте ваши месячные бюджеты"/>
        <div className="dashboard-grid">
          <StatCardBudget
            title="Общий бюджет"
            value={budgets ? formatCurrency(budgets.budgetSummaryDto.totalBudget) : "0,00 ₽"}
            variant="balance"
          />
          <StatCardBudget
            title="Расходы"
            value={budgets ? formatCurrency(budgets.budgetSummaryDto.totalSpent) : "0,00 ₽"}
            variant="expense"
          />
          <StatCardBudget
            title="Оставшийся бюджет"
            value={budgets ? formatCurrency(budgets.budgetSummaryDto.remaining) : "0,00 ₽"}
            variant="saving"
          />
        </div>
    <div className="budget-dashboard-layout">
          <BudgetCategories />
    </div>
      </div>
    );
};
export default BudgetPage;