import React from 'react';

function LayoutCentered({ children }: { children: React.ReactNode }) {
  return (
    <div className="flex justify-center p-5">
      {children}
    </div>
    );
}

export default LayoutCentered;