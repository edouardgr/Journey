using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class AI_Director : EditorWindow
{
    //General Window
    int move_state = 0; //0 = none, 1 = window, 2 = panel, 3 = context menu ;
    Vector2 mouse_origin, window_origin;
    Vector2 window_offset;

    //Panels
    AI_Graph graph;
    int select_index; //User clicked it
    int current_index; //User hovering over it
    Connector select_connector;
    Texture flow_connector;
    Connector connector;

    //ContextMenu
    Rect context_menu_rect = Rect.zero;
    string search_text;
    Texture search_icon;
    List<string> node_type_list;
    string[] black_list = { "Node", "Exit", "Init" };
    int list_index = -1;

    [MenuItem("Window/AI Director")]
    private static void OpenWindow()
    {
        AI_Director window = GetWindow<AI_Director>();
        window.titleContent = new GUIContent("AI Director");
    }

    void OnFocus()
    {
        Start();
    }

    private void OnGUI()
    {
        //Mouse
        Vector2 mouse = new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y);

        //Background stuff
        Rect window_rect = new Rect(position.x, position.y, position.width, position.height);
        EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), new Color(0.1f, 0.1f, 0.1f, 1f));

        if (graph != null) {

            //Check for right click
            if (Event.current.button == 1 && Event.current.type == EventType.MouseDown) {
                move_state = 3;
                window_origin = Event.current.mousePosition;
                select_index = -1;
                context_menu_rect = new Rect(window_origin, new Vector2(200, 300));
                search_text = "";
            }

            //Context menu is active
            if (move_state == 3) {
                //Search bar
                for (int i = 0; i < node_type_list.Count; i++) {
                    Rect item = new Rect(context_menu_rect.position + new Vector2(0, 20 * (i + 1)), new Vector2(context_menu_rect.size.x, 20));
                    if (item.Contains(new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y))) {
                        list_index = i;
                    }
                }
            }

            current_index = -1;
            for (int i = 0; i < graph.nodes.Count; i++) {
                if(new Rect(graph.nodes[i].position, graph.nodes[i].size).Contains(mouse - window_offset)) {
                    current_index = i;
                    break;
                }
            }

            //Get current node
            Node curr_node = (current_index >= 0 ? graph.nodes[current_index] : null);

            //Get current connector
            connector = null;
            if (current_index >= 0) {
                if (curr_node.has_input != null && curr_node.has_input.rect.Contains(mouse - curr_node.position - window_offset)) {
                    connector = curr_node.has_input;
                }
                if (curr_node.has_output != null && curr_node.has_output.rect.Contains(mouse - curr_node.position - window_offset)) {
                    connector = curr_node.has_output;
                }
            }

            //Check for clicked panel
            if (Event.current.button == 0 && Event.current.type == EventType.MouseDown) {
                //Context menu
                if (move_state == 3) {
                    if (!context_menu_rect.Contains(Event.current.mousePosition)) { //Disable if not clicked inside context menu
                        move_state = 0;
                    } else if (list_index >= 0) {
                        Node node;
                        //Creation of the nodes
                        switch (node_type_list[list_index]) {
                            case "Wait":
                            node = Node.Create<Wait>("Wait");
                            break;

                            case "Print":
                            node = Node.Create<Print>("Print");
                            break;


                            default: return;
                        }
                        node.position = mouse - window_offset;
                        graph.AddNode(node);
                        move_state = 0;
                    }
                }

                //Set pre-connector to select connector
                if (connector != null && select_connector == null) { //Create connection
                    select_connector = connector;
                    if (connector.dir == Direction.output) { //Check if we are leaving from the output
                        curr_node.next_node = null;
                    }
                } else if (connector == null && select_connector != null) { //Disable connection since nothing was clicked
                    select_connector = null;
                } else if (connector != null && select_connector != null) { //Confirmed connection//ADD DIRECTIONAL INPUT CAN ONLY ACCEPT OUTPUT AND THE OTHER WAY (TAGS)
                    if (select_connector.dir == Direction.output && connector.dir == Direction.input) {
                        graph.nodes[select_index].next_node = graph.nodes[graph.nodes.IndexOf(curr_node)];
                    } else if(select_connector.dir == Direction.input && connector.dir == Direction.output) {
                        curr_node.next_node = graph.nodes[select_index];
                    }
                    select_connector = null;

                }

                //Set pre-select to select
                if (current_index >= 0) {
                    select_index = current_index;
                } else {
                    select_index = -1;
                }
            }

            //Move the panel around
            if (move_state == 1 && Event.current.button == 0 && Event.current.type == EventType.MouseUp) {
                mouse_origin = Vector2.zero;
                window_origin = Vector2.zero;
                move_state = 0;
            } else if (move_state == 1) {
                graph.nodes[select_index].position = window_origin + (Event.current.mousePosition - mouse_origin);
            } else if (move_state == 0 && select_index >= 0 && Event.current.button == 0 && Event.current.type == EventType.MouseDown) {
                move_state = 1;
                mouse_origin = Event.current.mousePosition;
                window_origin = graph.nodes[select_index].position;
            }

            //Move the window offset around
            if (move_state == 2 && Event.current.button == 2 && Event.current.type == EventType.MouseUp) {
                mouse_origin = Vector2.zero;
                window_origin = Vector2.zero;
                move_state = 0;
            } else if (move_state == 2) {
                window_offset = window_origin + (Event.current.mousePosition - mouse_origin);
            } else if (move_state == 0 && Event.current.button == 2 && Event.current.type == EventType.MouseDown) {
                move_state = 2;
                mouse_origin = Event.current.mousePosition;
                window_origin = window_offset;
            }

            //Revert transform to zero
            if (Event.current.keyCode == KeyCode.Z) {
                window_offset = Vector2.zero;
            }

            //Centre panel
            if (select_index >= 0 && Event.current.keyCode == KeyCode.F) {
                window_offset = -graph.nodes[select_index].position - graph.nodes[select_index].size / 2 + position.size / 2;
            }

            //Remove node
            if (Event.current.keyCode == KeyCode.Delete && select_index >= 0 && !graph.nodes[select_index].is_protected) {
                for (int i = 0; i < graph.nodes.Count; i++) {
                    if(graph.nodes[i].next_node != null && graph.nodes[i].next_node.Equals(graph.nodes[select_index])) {
                        graph.nodes[i].next_node = null;
                    } 
                }
                DestroyImmediate(graph.nodes[select_index], true);
                graph.nodes.RemoveAt(select_index);
                select_index = -1;
            }

            //Draw Grid
            for (int i = (int)window_offset.x % 80; i < position.size.x; i += 80) { //X
                EditorGUI.DrawRect(new Rect(i, 0, 1, position.size.y), new Color(0, 0, 0, 0.4f));
            }
            for (int i = (int)window_offset.y % 80; i < position.size.y; i += 80) { //Y
                EditorGUI.DrawRect(new Rect(0, i, position.size.x, 1), new Color(0, 0, 0, 0.4f));
            }

            //Draw pre-select outline
            if (current_index >= 0 && current_index < graph.nodes.Count) {
                EditorGUI.DrawRect(new Rect(graph.nodes[current_index].position.x + window_offset.x - 2, graph.nodes[current_index].position.y + window_offset.y - 2, graph.nodes[current_index].size.x + 4, graph.nodes[current_index].size.y + 4), Color.gray); //Draw Selected panel rect
            }

            //Draw Connector lines
            if(select_connector != null && select_index >= 0) {
                Handles.DrawLine(select_connector.rect.position + graph.nodes[select_index].position + window_offset, mouse);
            }

            //Draw panels
            for (int i = 0; i < graph.nodes.Count; i++) {
                if (graph.nodes[i].has_input != null) { graph.nodes[i].has_input.active = false; }
                if (graph.nodes[i].has_output != null) { graph.nodes[i].has_output.active = false; }
            }

            for (int i = 0; i < graph.nodes.Count; i++) {
                if(graph.nodes[i].has_output != null) { graph.nodes[i].has_output.active = (graph.nodes[i].next_node != null /*|| (select_connector != null && select_index == i)*/); }
                if(graph.nodes[i].next_node != null) { graph.nodes[i].next_node.has_input.active = true; }
                EditorUtility.SetDirty(graph.nodes[i]);

                if (i != select_index) {
                    DrawPanel(graph.nodes[i], window_offset);
                }
            }
            
            //Draw outline and focused
            if (select_index >= 0 && select_index < graph.nodes.Count) {
                EditorGUI.DrawRect(new Rect(graph.nodes[select_index].position.x + window_offset.x - 2, graph.nodes[select_index].position.y + window_offset.y - 2, graph.nodes[select_index].size.x + 4, graph.nodes[select_index].size.y + 4), Color.blue); //Draw Selected panel rect
                DrawPanel(graph.nodes[select_index], window_offset);
            }

            //Draw pre-select for connectors
            if (connector != null) {
                EditorGUI.DrawRect(new Rect(connector.rect.position + curr_node.position + window_offset, connector.rect.size), Color.white);
            }

            //Draw Context Menu
            if (move_state == 3) {
                //Search bar and icon 
                EditorGUI.DrawRect(context_menu_rect, new Color(0.25f, 0.25f, 0.25f, 1f));
                GUI.DrawTexture(new Rect(context_menu_rect.position + Vector2.one, new Vector2(19, 19)), search_icon);
                GUILayout.BeginArea(new Rect(context_menu_rect.position + new Vector2(21, 1), new Vector2(context_menu_rect.size.x - 22, 19)));
                search_text = GUILayout.TextField(search_text);
                GUILayout.EndArea();
                //Behaviour list
                EditorGUI.DrawRect(new Rect(context_menu_rect.position + new Vector2(0, 20), context_menu_rect.size - new Vector2(0, 20)), new Color(0.3f, 0.3f, 0.3f, 1f));

                if (list_index >= 0) {
                    EditorGUI.DrawRect(new Rect(context_menu_rect.position + new Vector2(0, 20 * (list_index + 1)), new Vector2(context_menu_rect.size.x, 20)), new Color(0.4f, 0.4f, 0.4f, 1f));
                }

                for (int i = 0; i < node_type_list.Count; i++) {
                    EditorGUI.DrawRect(new Rect(context_menu_rect.position + new Vector2(0, 20 * (i + 2)), new Vector2(context_menu_rect.size.x, 1)), new Color(0, 0, 0, 0.4f));
                    GUILayout.BeginArea(new Rect(context_menu_rect.position + new Vector2(0, 20 * (i + 1)), new Vector2(context_menu_rect.size.x, 20)));
                    EditorGUILayout.LabelField(node_type_list[i]);
                    GUILayout.EndArea();
                }

            }
        }

        //Graph input field
        GUILayout.BeginArea(new Rect(Vector2.zero, new Vector2(200, 20)));
        //GUILayout.BeginHorizontal();
        graph = (AI_Graph)EditorGUILayout.ObjectField(graph, typeof(AI_Graph));
        //if(graph != null && GUILayout.Button("Save")) {

        //}
        //GUILayout.EndHorizontal();
        GUILayout.EndArea();
        
        Repaint(); //Repaint boio*/
    } 

    void Start()
    {
        mouse_origin = Vector2.zero;
        select_index = -1;
        list_index = -1;
        search_icon = Resources.Load<Texture2D>("Search");
        flow_connector = Resources.Load<Texture2D>("FlowConnector");
        node_type_list = new List<string>();
        foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/AI Graph/Node/")) {
            if (file.EndsWith(".cs")) {
                string term = file.Substring(file.LastIndexOf('/') + 1, file.Length - file.LastIndexOf('/') - 4);
                if (Array.IndexOf(black_list, term) < 0) {
                    node_type_list.Add(term);
                }
            }
        }
        window_offset = Vector2.zero;
    }

    void DrawPanel(Node node, Vector2 offset)
    {
        Rect _window = new Rect(node.position.x + offset.x, node.position.y + offset.y, node.size.x, node.size.y);
        Rect _header = new Rect(node.position.x + offset.x, node.position.y + offset.y, node.size.x, 20);
        
        //Draw Connectors
        if(node.next_node != null && node.next_node.has_input != null) {
            Handles.DrawLine(node.has_output.rect.position + _window.position, node.next_node.has_input.rect.position + node.next_node.position + offset);
        }

        EditorGUI.DrawRect(new Rect(_window.position - new Vector2(1, 1), _window.size + new Vector2(2, 2)), new Color(0.2f, 0.2f, 0.2f, 1f));
        EditorGUI.DrawRect(_window, Color.gray);
        EditorGUI.DrawRect(_header, node.color);

        GUILayout.BeginArea(_header);
        EditorGUILayout.LabelField(node.GetType().Name);
        GUILayout.EndArea();
        GUILayout.BeginArea(_window);
        //Input and Output connectors
        if (node.has_input != null) {
            node.has_input.Draw(flow_connector);
        }
        if (node.has_output != null) {
            node.has_output.Draw(flow_connector);
        }
        GUILayout.EndArea();
    }
}
