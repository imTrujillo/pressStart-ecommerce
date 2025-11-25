import React from "react";

export const Header = ({ title, subtitle, children }) => {
  return (
    <div className="page-header d-print-none">
      <div className="container-xl">
        <div className="row g-2 align-items-center d-flex flex-column flex-md-row">
          <div className="col">
            <h2 className="page-title">{title}</h2>
            <div className="page-pretitle">{subtitle}</div>
          </div>

          <div className="col-auto ms-auto d-print-none">
            <div className="btn-list">{children}</div>
          </div>
        </div>
      </div>
    </div>
  );
};
