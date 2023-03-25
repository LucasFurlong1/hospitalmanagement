import SideNav, { Toggle, NavItem, NavIcon, NavText } from '@trendmicro/react-sidenav';
import { useNavigate } from 'react-router-dom';
import '@trendmicro/react-sidenav/dist/react-sidenav.css';


const SideNavBar = () => {
    const navigate = useNavigate()
    return (
        <SideNav className="side_nav" onSelect={(selected) => {navigate('/'+selected)}}>
            <SideNav.Toggle />
            <SideNav.Nav defaultSelected="home">
                <NavItem eventKey="docpres">
                    <NavIcon><i className='fa-solid fa-pills' style={{ fontSize: "1.5rem" }} /></NavIcon>
                    <NavText><p style={{ fontFamily: 'monospace' }}>Prescriptions</p></NavText>
                </NavItem>
                <NavItem eventKey="docdiag">
                    <NavIcon><i className='fa-solid fa-paperclip' style={{ fontSize: "1.5rem" }} /></NavIcon>
                    <NavText><p style={{ fontFamily: 'monospace' }}>Diagnoses</p></NavText>
                </NavItem>
            </SideNav.Nav>
        </SideNav>
    )
}

export default SideNavBar;