function LayoutGrid({children}: {children: React.ReactNode}) {
    return (
        <div className="grid lg:grid-cols-3 md:grid-cols-2 sm:grid-cols-1 gap-x-2 gap-y-2 md:gap-x-4 md:gap-y-4 w-full max-w-4xl">
            {children}
        </div>
    )
}

export default LayoutGrid