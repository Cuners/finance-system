import './BudgetOverview.css';
interface BudgetItem {
  name: string;
  spent: number;
  budget: number;
  color: string;
}

const BudgetOverview = () => {
  const items: BudgetItem[] = [
    { name: 'Food & Dining', spent: 450, budget: 1000, color: '#ff764d' },
    { name: 'Transportation', spent: 200, budget: 400, color: '#3498db' },
    { name: 'Entertainment', spent: 150, budget: 200, color: '#9b59b6' },
    { name: 'Shopping', spent: 320, budget: 500, color: '#e74c3c' },
  ];

  return (
    <div className="budget-overview">
      <div className="section-header">
        <h3>Budget Overview</h3>
        <a href="#" className="manage-btn">Manage</a>
      </div>
      <div className="budget-list">
        {items.map((item, i) => (
          <div key={i} className="budget-item">
            <div className="budget-info">
              <div className="budget-name">{item.name}</div>
              <div className="budget-values">
                <span>${item.spent} / ${item.budget}</span>
              </div>
            </div>
            <div className="budget-bar">
              <div
                className="budget-progress"
                style={{
                  width: `${(item.spent / item.budget) * 100}%`,
                  backgroundColor: item.color,
                }}
              />
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default BudgetOverview;