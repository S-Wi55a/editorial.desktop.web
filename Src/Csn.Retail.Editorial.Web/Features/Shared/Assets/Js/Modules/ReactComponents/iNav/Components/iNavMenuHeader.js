import * as React from "react"
import INavMenuHeaderItem from './iNavMenuHeaderItem'

if (!SERVER) {
  require('../Css/iNav.MenuHeader.scss')  
}
//TODO: Covert to dumb component
class INavMenuHeader extends React.Component {

  constructor(props){
    super(props)
    this.state = {isVisible: null}
  }

  toggleVisibleItem = (id) => this.setState({isVisible:id})
    
  render({nodes}=this.props) {
    return (<div className={['iNav__menu-header'].join(' ')}>
      {nodes.map((node, index) => {
        return <INavMenuHeaderItem key={index} node={node} index={index} totalNodes={nodes.length} toggleVisibleItem={this.toggleVisibleItem} />
      })}
    </div>)
    }   
}

export default INavMenuHeader

