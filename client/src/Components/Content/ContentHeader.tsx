import './ContentHeader.css';
interface Header{
  title:string;
  description:string;
}
const TransactionsHeader = ({
    title,
    description
  }:Header) => {
  return (
    <div className="transactions-header">
      <div>
        <h1>{title}</h1>
        <p>{description}</p>
      </div>
      <button className="add-btn">+ Add Transaction</button>
    </div>
  );
};

export default TransactionsHeader;