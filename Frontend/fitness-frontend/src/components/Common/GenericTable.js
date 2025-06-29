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
      <table
        className="generic-table"
        style={{
          width: "100%",
          borderCollapse: "collapse",
          backgroundColor: "#fff",
        }}
      >
        <thead>
          <tr>
            {columns.map((col) => (
              <th
                key={col}
                onClick={() => handleSort(col)}
                style={{
                  cursor: store?.setSort ? "pointer" : "default",
                  userSelect: "none",
                  backgroundColor: "#fff",
                  color: "#000",
                  padding: "0.75rem",
                  textAlign: "left",
                  borderBottom: "1px solid #ccc",
                }}
              >
                {col}
                {store?.sortBy === col
                  ? store.sortOrder?.toLowerCase() === "asc"
                    ? " ↑"
                    : " ↓"
                  : ""}
              </th>
            ))}
            {headerButton && (
              <th
                style={{
                  textAlign: "right",
                  minWidth: 120,
                  backgroundColor: "#fff",
                }}
              >
                {headerButton}
              </th>
            )}
            {onDeleteRow && (
              <th style={{ width: 30, backgroundColor: "#fff" }}></th>
            )}
          </tr>
        </thead>
        <tbody>
          {data.map((item, idx) => (
            <tr
              key={item.Id || idx}
              onClick={() => onRowClick?.(item)}
              className={onRowClick ? "clickable" : ""}
              style={{
                cursor: onRowClick ? "pointer" : "default",
                backgroundColor: "#fff",
                color: "#000",
              }}
            >
              {columns.map((col) => (
                <td
                  key={col}
                  style={{
                    padding: "0.75rem",
                    borderBottom: "1px solid #eee",
                    color: "#000",
                  }}
                >
                  {String(item[col])}
                </td>
              ))}
              {headerButton && <td style={{ backgroundColor: "#fff" }}></td>}
              {onDeleteRow && (
                <td style={{ backgroundColor: "#fff" }}>
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      onDeleteRow(item.Id);
                    }}
                    style={{
                      color: "#fff",
                      background: "#000",
                      border: "1px solid #000",
                      fontWeight: "bold",
                      fontSize: "1.2em",
                      cursor: "pointer",
                      padding: "0.25rem 0.6rem",
                      borderRadius: "4px",
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
        <div
          className="generic-table-pagination"
          style={{
            backgroundColor: "#fff",
            color: "#000",
            padding: "1rem",
            textAlign: "center",
            marginTop: "16px",
          }}
        >
          <div
            style={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              gap: "1rem",
            }}
          >
            <button
              onClick={() => handlePageChange(store.currentPage - 1)}
              disabled={store.currentPage <= 1}
              style={{
                backgroundColor: "#fff",
                color: "#000",
                border: "1px solid #fff",
                padding: "0.5rem 0.7rem",
                fontSize: "20px",
                cursor: "pointer",
              }}
            >
              ◀
            </button>
            <span style={{ fontSize: "18px", fontWeight: 600 }}>
              Page {store.currentPage} / {store.totalPages}
            </span>
            <button
              onClick={() => handlePageChange(store.currentPage + 1)}
              disabled={store.currentPage >= store.totalPages}
              style={{
                backgroundColor: "#fff",
                color: "#000",
                padding: "0.5rem 0.7rem",
                border: "1px solid #fff",
                fontSize: "20px",
                cursor: "pointer",
              }}
            >
              ▶
            </button>
          </div>
        </div>
      )}
    </div>
  );
})
export default GenericTable;
