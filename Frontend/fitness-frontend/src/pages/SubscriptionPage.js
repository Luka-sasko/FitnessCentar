import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { subscriptionStore } from "../stores/SubscriptionStore";
import "../styles/subscription.css"; 

const SubscriptionPage = observer(() => {
  useEffect(() => {
    subscriptionStore.fetchAll();
  }, []);

  return (
    <div className="subscription-container">
      {subscriptionStore.subscriptionList.map((sub) => (
        <div className="subscription-card" key={sub.Id}>
          <h3>{sub.Name}</h3>
          <p>Price: ${sub.Price}</p>
          <p>Duration: {sub.Duration} days</p>
        </div>
      ))}
    </div>
  );
});

export default SubscriptionPage;
