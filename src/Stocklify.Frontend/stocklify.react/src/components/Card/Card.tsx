import './Card.css'

function Card({children}: {children?: React.ReactNode}) {
    return (
        <>
            <div className="card h-40 p-3 rounded-xl border-1">
                {children}
            </div>
        </>
    )
}

export default Card