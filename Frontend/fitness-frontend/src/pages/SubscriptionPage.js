import React, { useEffect } from "react";
import { toJS } from "mobx";
import { subscriptionStore } from "../stores/SubscriptionStore";

const SubscriptionPage = () => {
  useEffect(() => {
    const fetchData = async () => {
      await subscriptionStore.fetchAll();
      console.log("Fetched Subscription list:", toJS(subscriptionStore.subscriptionList));      
      console.log("PagedMeta:", toJS(subscriptionStore.pagedMeta));
      
    };
    fetchData();
  }, []);

  return (
    <div>
      <h2>Subscription Page</h2>
    </div>
  );
};

export default SubscriptionPage;
