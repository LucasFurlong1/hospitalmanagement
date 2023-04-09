import SideNav, { Toggle, NavItem, NavIcon, NavText } from '@trendmicro/react-sidenav';
import { useNavigate, useLocation } from 'react-router-dom';
import '@trendmicro/react-sidenav/dist/react-sidenav.css';


const SideNavBarPat = () => {
    const navigate = useNavigate()
    return (
        <SideNav className="side_nav" onSelect={(selected) => {navigate(`/${selected}`, {state: {}})}}>
            <SideNav.Toggle />
            <SideNav.Nav defaultSelected="home">
                <NavItem eventKey="patient-info">
                    <NavIcon><i className='fa-solid fa-clipboard' style={{ fontSize: "1.5rem" }} /></NavIcon>
                    <NavText><p style={{ fontFamily: 'monospace' }}>Information</p></NavText>
                </NavItem>
            </SideNav.Nav>
        </SideNav>
    )
}

export default SideNavBarPat;