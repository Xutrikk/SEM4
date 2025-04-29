import { BrowserRouter, Routes, Route } from 'react-router-dom';
import SignIn from './components/SignIn/SignIn';
import SignUp from './components/SignUp/SignUp';
import ResetPassword from './components/ResetPassword/ResetPassword';

if (!localStorage.getItem('users')) {
  localStorage.setItem('users', JSON.stringify([]));
}

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/sign-in" element={<SignIn />} />
        <Route path="/sign-up" element={<SignUp />} />
        <Route path="/reset-password" element={<ResetPassword />} />
        <Route path="/*" element={<SignIn />} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;