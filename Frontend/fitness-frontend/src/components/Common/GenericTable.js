import React from "react";
import { observer } from "mobx-react-lite";

const GenericTable = observer(({ store, items, onRowClick, headerButton, onDeleteRow }) => {
  const data = items ?? store?.items ?? [];

  const hiddenColumns = [
    "Id",
    "DateCreated",
    "UpdatedBy",
    "CreatedBy",
    "UpdatedAt",
    "DateUpdated",
    "IsActive",
    "UserId",
    "StartDate",
    "EndDate",
    "id"
  ];

  const columns = Object.keys(data[0] || {}).filter(
    (key) => !hiddenColumns.includes(key)
  );

  const handleSort = (col) => {
    if (!store?.setSort) return;
    const newOrder = store.sortBy === col && store.sortOrder?.toLowerCase() === "asc" ? "desc" : "asc";
    store.setSort(col, newOrder);
  };

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= store.totalPages && newPage !== store.currentPage) {
      store.setCurrentPage(newPage);
    }
  };

  return (
    <div className="generic-table-container">
      <table className="generic-table">
        <thead>
          <tr>
            {columns.map((col) => (
              <th
                key={col}
                onClick={() => handleSort(col)}
              >
                {col}
                {store?.sortBy === col
                  ? store.sortOrder?.toLowerCase() === "asc"
                    ? " ↑"
                    : " ↓"
                  : ""}
              </th>
            ))}
            {headerButton && <th>{headerButton}</th>}
            {onDeleteRow && <th></th>}
          </tr>
        </thead>
        <tbody>
          {data.map((item, idx) => (
            <tr
              key={item.Id || idx}
              onClick={() => onRowClick?.(item)}
              className={onRowClick ? "clickable" : ""}
            >
              {columns.map((col) => (
                <td key={col}>
                  {String(item[col])}
                </td>
              ))}
              {headerButton && <td></td>}
              {onDeleteRow && (
                <td>
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      onDeleteRow(item.Id);
                    }}
                    title="Delete"
                  >
                    ×
                  </button>
                </td>
              )}
            </tr>
          ))}
        </tbody>
      </table>

      {store && (
        <div className="generic-table-pagination">
          <button
            onClick={() => handlePageChange(store.currentPage - 1)}
            disabled={store.currentPage <= 1}
          >
            ◀
          </button>
          <span>
            Page {store.currentPage} / {store.totalPages}
          </span>
          <button
            onClick={() => handlePageChange(store.currentPage + 1)}
            disabled={store.currentPage >= store.totalPages}
          >
            ▶
          </button>
        </div>
      )}
    </div>
  );
});

export default GenericTable;
