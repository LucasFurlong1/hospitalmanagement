import SideNav, { Toggle, NavItem, NavIcon, NavText } from '@trendmicro/react-sidenav';
import { useNavigate } from 'react-router-dom';
import '@trendmicro/react-sidenav/dist/react-sidenav.css';

let username = ""
const SideNavBarDoc = (props) => {

    const navigate = useNavigate()
    username = props.props
    return (
        <SideNav className="side_nav" onSelect={(selected) => {navigate(`/${selected}`, {state: {username}})}}>
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

export default SideNavBarDoc;