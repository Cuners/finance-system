import { useState } from 'react';
import StatCardBudget from "./StatCardBudget";
import "./BudgetPage.css";
import {useBudgetsSummary} from '../../../Hooks/useBudgetsSummary';
import BudgetCategories from "./BudgetCategories";
import TransactionsHeader from "../ContentHeader.tsx";
import AddBudgetModal from "./BudgetAddUpdate";
import { formatCurrency } from '../../../Utils/formatUtils';

const BudgetPage = () => {
    const { budgets, loading, error } = useBudgetsSummary();
    const [isModalOpen, setIsModalOpen] = useState(false);

    const handleOpenModal = () => {
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
    };

    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;
    return(
      <div className="budget-page">
        <div className="header-controls">
          <TransactionsHeader
            title="Бюджеты"
            description="Планируйте и отслеживайте ваши месячные бюджеты"/>
          <button className="add-btn" onClick={handleOpenModal}>+ Add Budget</button>
        </div>
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
      {isModalOpen && (
        <AddBudgetModal
          isOpen={isModalOpen}
          onClose={handleCloseModal}
          mode="create"
          onSuccess={() => {
          }}
        />
      )}
      </div>
    );
};
export default BudgetPage;