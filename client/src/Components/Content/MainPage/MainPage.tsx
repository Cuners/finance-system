import StatCard from "../MainPage/StatCard";
import RecentTransactions from "../MainPage/RecentTransactions";
import BudgetOverview from "../MainPage/BudgetOverview";
import "./MainPage.css";
import {useDashboardData} from '../../../Hooks/useDashboardData';
import TransactionsHeader from "../ContentHeader";
import TransactionTopExpenses from "./TransactionTopExpenses";
import TransactionUpcomingBills from "./TransactionUpcomingBill";
const MainPage = () => {
    const now = new Date();
    const { data, loading, error } = useDashboardData(now.getFullYear(), now.getMonth()+1);
    const formatCurrency = (value: number) => 
      new Intl.NumberFormat('ru-RU', {style: 'currency',currency: 'RUB'}).format(value);
    const formatter=new Intl.DateTimeFormat('ru-RU', { month: 'long' });
    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;
    return(
      <div className="main-page">
        <TransactionsHeader
            title='Добро пожаловать обратно'
            description={`Ваши финансовые данные на ${formatter.format()} 2026`}/>
        <div className="dashboard-grid">
          <StatCard
            title="Общий баланс"
            value={data ? formatCurrency(data.totalBalance) : "0,00 ₽"}
            changeValue={12.5}
            variant="balance"
          />
          <StatCard
            title="Доходы"
            value={data ? formatCurrency(data.totalIncome) : "0,00 ₽"}
            changeValue={8.2}
            variant="income"
          />
          <StatCard
            title="Расходы"
            value={data ? formatCurrency(data.totalExpenses) : "0,00 ₽"}
            changeValue={-3.1}
            variant="expense"
          />
         <StatCard
            title="Сбережения"
            value={data ? formatCurrency(data.savings) : "0,00 ₽"}
            changeValue={-3.1}
            variant="saving"
          />
        </div>
        <div className="dashboard-layout">
            {/* <RecentTransactions /> */}
            <RecentTransactions />
              <BudgetOverview />
              <TransactionTopExpenses/>
              <TransactionUpcomingBills/>
        </div>
      </div>
    );
};
export default MainPage;