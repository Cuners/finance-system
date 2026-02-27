import './InputField.css';

interface InputFieldProps {
  label: string;
  id: string;
  type?: 'text' | 'email' | 'password' | 'number';
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  required?: boolean;
}

const InputField = ({
  label,
  id,
  type = 'text',
  value,
  onChange,
  placeholder,
  required = false,
}: InputFieldProps) => {
  return (
    <div className="input-group">
      <label htmlFor={id}>{label}</label>
      <input
        id={id}
        type={type}
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder={placeholder}
        required={required}
      />
    </div>
  );
};

export default InputField;