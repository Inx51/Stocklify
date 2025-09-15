import { useState } from "react";
import { JSX } from "react";
import ToolTip from "../ToolTip/ToolTip";
import './ToolButton.css';

function ToolButton({Icon, ActiveIcon, Tooltip}: {Icon: JSX.Element, ActiveIcon: JSX.Element, Tooltip: string}) { 

    const [hover, setHover] = useState(false);
    const [active, setActive] = useState(false);

    return (
        <div 
        onMouseEnter={() => setHover(true)}
        onMouseLeave={(() => setHover(false))}
        onClick={(() => setActive(!active))}
        className={`p-1 button ${active ? "button-active" : ""} ${hover ? "button-hover" : ""}`} aria-label="{Tooltip}">
            {active ? ActiveIcon : Icon}
            <ToolTip text={Tooltip} visible={hover} />
        </div>
    );
}

export default ToolButton;