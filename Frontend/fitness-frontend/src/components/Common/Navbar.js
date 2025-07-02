import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import logo from '../../assets/icons/barbell.png';
import '../../styles/Navbar.css';
import { observer } from 'mobx-react-lite';
import { userStore } from '../../stores/UserStore';

const links = [
  { name: 'Home', path: '/home' },
  { name: 'Discounts', path: '/discounts', adminOnly: true },
  { name: 'Meal Plans', path: '/mealplans' },
  { name: 'Subscriptions', path: '/subscriptions' },
  { name: 'Workout Plans', path: '/workoutplans' },
  { name: 'Exercises', path: '/exercises' },
];

const Navbar = observer(() => {
  const navigate = useNavigate();

  const handleLogout = () => {
    userStore.logout();
    navigate("/login");
  };

  return (
    <nav className="navbar">
      <img src={logo} alt="App logo" className="navbar-logo" />
      <>
        {userStore.isLoggedIn && (
          <div className="navbar-left">
            <ul className="navbar-links">
              {links.map(link => {
                if (link.adminOnly && !userStore.isAdmin) return null;
                return (
                  <li key={link.path}>
                    <NavLink
                      to={link.path}
                      className={({ isActive }) => (isActive ? 'active' : '')}
                    >
                      {link.name.toLocaleUpperCase()}
                    </NavLink>
                  </li>
                );
              })}

            </ul>
          </div>
        )}
      </>


      <ul className="navbar-right">
        {!userStore.isLoggedIn ? (
          <>
            <li><NavLink to="/">HOME</NavLink></li>
            <li><NavLink to="/login">LOGIN</NavLink></li>
            <li><NavLink to="/register">REGISTER</NavLink></li>
          </>
        ) : (
          <>
            <li><NavLink to="/profile">{userStore.currentUser?.username || 'Profile'}</NavLink></li>
            <li><button onClick={handleLogout} className="logout-btn">LOGOUT</button></li>
          </>
        )}
      </ul>
    </nav>
  );
});

export default Navbar;
