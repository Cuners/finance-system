import "./AuthorizationPage.css"
import InputField from "./AuthItems/InputField";
import React, { useState } from 'react';
import { Link } from 'react-router';
const AuthorizationPage = () => {
    const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log('Email:', email, 'Password:', password);
  };
    return(
    <div className="auth-container">
      <form className="auth-form" onSubmit={handleSubmit}>
        <h2>Авторизация</h2>

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
          Войти
        </button>

        <p className="register-link">
          Не зарегистрированы? <Link to="/register">Создайте аккаунт</Link>
        </p>
      </form>
    </div>
    );
};
export default AuthorizationPage;