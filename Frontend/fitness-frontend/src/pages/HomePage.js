import { useEffect } from 'react';
import { Link } from 'react-router-dom';
import { observer } from "mobx-react-lite";
import { userStore } from "../stores/UserStore";

const HomePage = observer(() => {
  useEffect(() => {
    userStore.fetch();
  }, []);

  const pages = [
  ...(userStore.isAdmin ? [{ name: "Discounts", path: "/discounts" }] : []),
  { name: "Subscriptions", path: "/subscriptions" },
  { name: "Meal Plans", path: "/mealplans" },
  { name: "Workout Plans", path: "/workoutplans" },
  { name: "Exercises", path: "/exercises" },
];


  const filteredPages = pages.filter(p => {
    if (p.name === "Discounts" && !userStore.isAdmin) return false;
    return true;
  });

  return (
    <div style={{ padding: '20px', alignContent: 'center' }}>
      <h1 style={{ display: 'flex', alignItems: 'center', gap: '10px', fontSize: '2.2rem' }}>
        <span role="img" aria-label="dumbbell">🏋️‍♂️</span>
        <span style={{ fontWeight: 'bold', color: '#2c3e50' }}>FITNESS CENTAR</span>
      </h1>

      <div style={styles.grid}>
        {filteredPages.map((page) => (
          <Link to={page.path} key={page.path} style={styles.card}>
            <h3>{page.name}</h3>
          </Link>
        ))}
      </div>
    </div>
  );
});
const styles = {
  grid: {
  display: 'flex',
  flexDirection: 'column',
  gap: '16px',
  marginTop: '20px',
  alignItems: 'center'
},

  card: {
  marginTop: "1%",
  textDecoration: 'none',
  padding: '20px',
  border: '1px solid #61dafb',
  borderRadius: '10px',
  backgroundColor: '#fff',
  color: '#000',
  textAlign: 'center',
  transition: 'transform 0.2s, box-shadow 0.2s',
  boxShadow: '0 2px 5px rgba(0,0,0,0.1)',
  width: '30%' // fiksna širina
},

};

export default HomePage;
