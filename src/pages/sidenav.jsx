import SideNav, {Toggle, NavItem, NavIcon, NavText} from '@trendmicro/react-sidenav';
import '@trendmicro/react-sidenav/dist/react-sidenav.css';


const SideNavBar = () => {
    return (
        <SideNav className="side_nav">
            <SideNav.Toggle/>
            <SideNav.Nav defaultSelected="home">
                <NavItem>
                    <NavIcon><i className='fa-solid fa-pills' style={{fontSize: "1.5rem"}}/></NavIcon>
                    <NavText><p style={{fontFamily: 'monospace'}}>Prescriptions</p></NavText>
                </NavItem>
                <NavItem>
                    <NavIcon><i className='fa-solid fa-paperclip' style={{fontSize: "1.5rem"}}/></NavIcon>
                    <NavText><p style={{fontFamily: 'monospace'}}>Diagnoses</p></NavText>
                </NavItem>
            </SideNav.Nav>
        </SideNav>
    )
}

export default SideNavBar;