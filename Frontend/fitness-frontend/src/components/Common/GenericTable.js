import React from "react";
import { observer } from "mobx-react-lite";

const GenericTable = observer(({ store, onRowClick, headerButton, onDeleteRow }) => {
    const data = store.items ?? [];

    if (!data || data.length === 0) return <p>🔍 No data found.</p>;

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
        const newOrder = store.sortOrder.toLowerCase() === "asc" ? "desc" : "asc";
        store.setSort(col, newOrder);
    };

    const handlePageChange = (newPage) => {
        if (
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
                                    cursor: "pointer",
                                    userSelect: "none",
                                }}
                            >
                                {col}{" "}
                                {store.sortBy === col
                                    ? store.sortOrder.toLowerCase() === "asc"
                                        ? "↑"
                                        : "↓"
                                    : ""}
                            </th>
                        ))}
                        {headerButton && <th style={{ textAlign: "right", minWidth: 120 }}>{headerButton}</th>}
                        {onDeleteRow && <th style={{ width: 30 }}></th>}
                    </tr>
                </thead>
                <tbody>
                    {data.map((item) => (
                        <tr
                            key={item.Id}
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
            <div className="generic-table-pagination">
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
    );
});

export default GenericTable;
