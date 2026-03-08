import React, { useEffect, useState } from 'react';
import WalletHistory from './WalletHistory';
import { useRecentTransactions } from '../../../Hooks/useTransactions';
import { type Transaction } from './WalletHistory';
import { type TransactionDto } from '../../../Types';

interface WalletHistoryModalProps {
  isOpen: boolean;
  onClose: () => void;
  walletId: number;
  walletTitle: string;
}

// Convert TransactionDto to Transaction format
const mapTransactionDtoToTransaction = (dto: TransactionDto): Transaction => {
  return {
    id: dto.transactionId.toString(),
    title: dto.categoryName,
    category: dto.categoryName,
    date: new Date(dto.date).toLocaleDateString('en-GB'),
    amount: dto.amount,
    status: 'completed',
    type: dto.amount >= 0 ? 'income' : 'expense',
    icon: undefined,
  };
};

const WalletHistoryModal: React.FC<WalletHistoryModalProps> = ({ 
  isOpen, 
  onClose, 
  walletId, 
  walletTitle 
}) => {

  const { data: transactionDtos, loading, error } = useRecentTransactions({ accountId: walletId });

  const [transactions, setTransactions] = useState<Transaction[]>([]);

  useEffect(() => {
    if (transactionDtos) {
      const mappedTransactions = transactionDtos.map(mapTransactionDtoToTransaction);
      setTransactions(mappedTransactions);
    }
  }, [transactionDtos]);

  if (!isOpen) {
    return null;
  }

  return (
    <WalletHistory
      isOpen={isOpen}
      onClose={onClose}
      walletTitle={walletTitle}
      transactions={transactions}
    />
  );
};

export default WalletHistoryModal;