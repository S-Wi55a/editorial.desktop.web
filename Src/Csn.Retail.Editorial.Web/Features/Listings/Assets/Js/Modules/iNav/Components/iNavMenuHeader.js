import * as React from "react"
import INavMenuHeaderItem from 'iNav/Components/iNavMenuHeaderItem'

if (!SERVER) {
  require('iNav/Css/iNav.MenuHeader.scss')  
}

class INavMenuHeader extends React.Component {

  constructor(props){
    super(props)
    this.state = {isVisible: null}
  }

  toggleVisibleItem = (id) => this.setState({isVisible:id})
    
  render({nodes}=this.props) {
    return (<div className={['iNav__menu-header'].join(' ')}>
      {nodes.map((node, index) => {
        return <INavMenuHeaderItem key={index} node={node} index={index} toggleVisibleItem={this.toggleVisibleItem} />
      })}
    </div>)
    }   
}

export default INavMenuHeader

