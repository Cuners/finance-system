import "./AuthorizationPage.css"
import InputField from "./AuthItems/InputField";
import React, { useState } from 'react';
import { Link } from 'react-router';
const RegistrationPage = () => {
    const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
   const [login, setLogin] = useState<string>('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Здесь логика регистрации
    console.log('Login:',login,'Email:', email, 'Password:', password);
  };
    return(
    <div className="auth-container">
      <form className="auth-form" onSubmit={handleSubmit}>
        <h2>Регистрация</h2>
       <InputField
          label="Login"
          id="login"
          type="text"
          value={login}
          onChange={setLogin}
          placeholder="Введите ваш логин"
          required
        />

       <InputField
          label="Email"
          id="email"
          type="email"
          value={email}
          onChange={setEmail}
          placeholder="Введите ваш email"
          required
        />

        <InputField
          label="Пароль"
          id="password"
          type="password"
          value={password}
          onChange={setPassword}
          placeholder="Введите пароль"
          required
        />

        <button type="submit" className="login-button">
          Зарегистрироваться
        </button>

        <p className="register-link">
          Уже авторизован? <Link to="/auth">Войдите в аккаунт</Link>
        </p>
      </form>
    </div>
    );
};
export default RegistrationPage;