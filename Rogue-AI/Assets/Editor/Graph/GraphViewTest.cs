#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphViewTest : EditorWindow
{
	private GraphViewTestView graphView;

	[MenuItem("Tools/Graph View Test")]
	public static void OpenWindow()
	{
		var window = GetWindow<GraphViewTest>();
		window.titleContent = new GUIContent("Graph View Test");
	}

	private void OnEnable()
	{
		ConstructGraphView();
		GenerateToolbar();
	}

	private void OnDisable()
	{
		rootVisualElement.Remove(graphView);
	}

	private void ConstructGraphView()
	{
		graphView = new GraphViewTestView
		{
			name = "Graph View"
		};
		graphView.StretchToParentSize();
		rootVisualElement.Add(graphView);
	}

	private void GenerateToolbar()
	{
		var toolbar = new Toolbar();

		var nodeNameField = new TextField("Node ID");
		nodeNameField.value = "Node_" + Random.Range(1, 1000);
		toolbar.Add(nodeNameField);

		var addButton = new Button(() => graphView.CreateNode(nodeNameField.value))
		{
			text = "Add Node"
		};
		toolbar.Add(addButton);

		rootVisualElement.Add(toolbar);
	}

	public class GraphViewTestView : GraphView
	{
		public GraphViewTestView()
		{
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
			
			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();
		}

		public void CreateNode(string nodeId)
		{
			var node = new StoryNodeView(nodeId);
			AddElement(node);
		}
	}

	public class StoryNodeView : Node
	{
		public StoryNodeView(string nodeId)
		{
			title = nodeId;
			style.width = 200;

			var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
			inputPort.portName = "In";
			inputContainer.Add(inputPort);

			var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
			outputPort.portName = "Out";
			outputContainer.Add(outputPort);

			var textField = new TextField("Text");
			textField.multiline = true;
			mainContainer.Add(textField);

			RefreshExpandedState();
			RefreshPorts();
			SetPosition(new Rect(Vector2.zero, new Vector2(200, 150)));
		}
	}
}
#endif