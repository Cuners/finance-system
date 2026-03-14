import "./AuthorizationPage.css"
import InputField from "./AuthItems/InputField";
import React, { useState } from 'react';
import { Link } from 'react-router';
import { useLogin } from "../../../Hooks/useLogin";
import { type LoginRequest } from "../../../Types";
const AuthorizationPage = () => {
    const [username, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const { isLoading, error, login } = useLogin();
const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    const loginData: LoginRequest = { username, password };
    
    try {
      await login(loginData);
    } catch {

    }
  };
    return(
    <div className="auth-container">
      <form className="auth-form" onSubmit={handleSubmit}>
        <h2>Авторизация</h2>
        {error && (
          <div className="error-message" role="alert">
            {error}
          </div>
        )}
       <InputField
          label="Email"
          id="email"
          type="text"
          value={username}
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

        <button 
          type="submit" 
          className="login-button"
          disabled={isLoading || !username || !password}
        >
          {isLoading ? 'Вход...' : 'Войти'}
        </button>

        <p className="register-link">
          Не зарегистрированы? <Link to="/register">Создайте аккаунт</Link>
        </p>
      </form>
    </div>
    );
};
export default AuthorizationPage;