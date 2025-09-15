import './ToolTip.css';

function ToolTip({text, visible}: {text: string, visible: boolean}) {
    return (
        <div 
        className={`tooltip text-gray-300 text-xs p-2 rounded mt-4 ${visible ? 'tooltip-visible' : 'tooltip-hidden'}`}>
            {text}
        </div>
    );
}

export default ToolTip;