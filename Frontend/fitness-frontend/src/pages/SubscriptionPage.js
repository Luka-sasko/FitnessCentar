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
      {subscriptionStore.subscriptionList.map((sub) => {
        console.log(sub);
        const nameLower = sub.Name.toLowerCase();
        const cardClass =
          nameLower.includes("gold")
            ? "gold-plan"
            : nameLower.includes("basic")
              ? "basic-plan"
              : "";

        return (
          <div className={`subscription-card ${cardClass}`} key={sub.Id}>
            <h3>{sub.Name}</h3>
            <p>PRICE: ${sub.Price}</p>
            <p>START DATE: {new Date(sub.StartDate).toLocaleDateString('en-GB', {
              day: '2-digit',
              month: '2-digit',
              year: 'numeric'
            })}</p>
          </div>

        );
      })}
    </div>
  );


});

export default SubscriptionPage;
