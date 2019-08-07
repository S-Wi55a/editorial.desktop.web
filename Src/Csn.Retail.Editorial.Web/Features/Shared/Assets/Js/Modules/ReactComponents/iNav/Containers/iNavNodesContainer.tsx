import React from 'react'
import INavNodeContainer from '../Containers/iNavNodeContainer'
import { INode } from '../Types'

if (!SERVER) {
  require('../Css/iNav.NodesContainer')  
}

interface IINavNodesContainer {
  nodes: INode[] 
  activeItemId: number
}

const INavNodesContainer: React.StatelessComponent<IINavNodesContainer> = ({ nodes, activeItemId }) => (
  <div className={'iNav__category iNav-category'}>
    {nodes.map((node, index) => {
      return <INavNodeContainer 
        key={node.name} 
        {...node} 
        activeItemId={activeItemId} 
        index={index} 
        activePage={1}
        refinementId={null}
        isLoading={false}
        totalNodes={nodes.length}
        />
    })}
  </div>
);

// Connect the Component to the store
export default INavNodesContainer