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
    <div style={{ textAlign: 'center', padding: '40px', backgroundColor: '#FD9242' }}>
      <h1 style={{ display: 'inline-flex', alignItems: 'center', gap: '10px', fontSize: '2.2rem' }}>
        <span style={{ fontWeight: 'bold', color: '#1f2937' }}>FITNESS CENTAR</span>
      </h1>

      <div style={styles.grid}>
        {filteredPages.map((page) => (
          <Link to={page.path} key={page.path} style={styles.card}>
            <div style={styles.banner}></div> { }
            <h3 style={styles.cardTitle}>{page.name.toUpperCase()}</h3>
          </Link>
        ))}
      </div>
    </div>
  );
});

const styles = {
  grid: {
    display: 'flex',
    flexWrap: 'wrap',
    gap: '20px',
    justifyContent: 'center',
    marginTop: '30px',
  },

  card: {
    position: 'relative',
    textDecoration: 'none',
    padding: '30px 20px 20px 20px',
    borderRadius: '10px',
    backgroundColor: 'rgba(248, 250, 252, 0.3)',
    color: '#1f2937',
    textAlign: 'center',
    transition: 'transform 0.2s, box-shadow 0.2s',
    width: '250px',
    boxShadow: '0 4px 8px rgba(0,0,0,0.05)',
  },

  cardTitle: {
    marginTop: '10px',
    fontSize: '1.1rem',
    fontWeight: 'bold',
  },

  banner: {
    position: 'absolute',
    top: 0,
    left: 0,
    height: '6px',
    width: '100%',
    backgroundColor: '#fff',
    borderTopLeftRadius: '10px',
    borderTopRightRadius: '10px',
  }
};

export default HomePage;
