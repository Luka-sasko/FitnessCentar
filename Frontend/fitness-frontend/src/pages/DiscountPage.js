import React, { useEffect } from "react";
import { discountStore } from "../stores/DiscountStore";
import { toJS } from "mobx";


const DiscountPage = () => {
  useEffect(() => {
    const fetchData = async () => {
      await discountStore.fetchAll();
      console.log("Discounts:", toJS(discountStore.discountList));
      console.log("PagedMeta:", toJS(discountStore.pagedMeta));
    };
    fetchData();
  }, []);

  return (
    <div>
      <h2>Discount Page</h2>
    </div>
  );
};

export default DiscountPage;
