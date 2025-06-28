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
        if (
            store &&
            newPage >= 1 &&
            newPage <= store.totalPages &&
            newPage !== store.currentPage
        ) {
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
                                style={{
                                    cursor: store?.setSort ? "pointer" : "default",
                                    userSelect: "none",
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
                        {headerButton && <th style={{ textAlign: "right", minWidth: 120 }}>{headerButton}</th>}
                        {onDeleteRow && <th style={{ width: 30 }}></th>}
                    </tr>
                </thead>
                <tbody>
                    {data.map((item, idx) => (
                        <tr
                            key={item.Id || idx}
                            onClick={() => onRowClick?.(item)}
                            className={onRowClick ? "clickable" : ""}
                            style={{ cursor: onRowClick ? "pointer" : "default" }}
                        >
                            {columns.map((col) => (
                                <td key={col}>{String(item[col])}</td>
                            ))}
                            {headerButton && <td></td>}
                            {onDeleteRow && (
                                <td>
                                    <button
                                        onClick={e => { e.stopPropagation(); onDeleteRow(item.Id); }}
                                        style={{
                                            color: "#e03c3c",
                                            background: "none",
                                            border: "none",
                                            fontWeight: "bold",
                                            fontSize: "1.2em",
                                            cursor: "pointer",
                                            padding: 0
                                        }}
                                        title="Delete"
                                    >×</button>
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </table>

            {store && (
                <div className="generic-table-pagination">
                    
                    <div style={{ display: "flex", gap: 8, alignItems: "center", marginTop: 5 }}>
                        <button
                            onClick={() => handlePageChange(store.currentPage - 1)}
                            disabled={store.currentPage <= 1}
                        >
                            ⬅️
                        </button>
                        <span>
                            Page {store.currentPage} / {store.totalPages}
                        </span>
                        <button
                            onClick={() => handlePageChange(store.currentPage + 1)}
                            disabled={store.currentPage >= store.totalPages}
                        >
                            ➡️
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
});

export default GenericTable;
