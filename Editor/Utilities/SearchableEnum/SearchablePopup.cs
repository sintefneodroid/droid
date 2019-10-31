using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System;
using UnityEditor;

namespace droid.Editor.Utilities.SearchableEnum {
  /// <inheritdoc />
  /// <summary>
  ///   A popup window that displays a list of options and may use a search
  ///   string to filter the displayed content.
  /// </summary>
  public class SearchablePopup : PopupWindowContent {
    #region -- Initialization ---------------------------------------------

    SearchablePopup(string[] names, int current_index, Action<int> on_selection_made) {
      this._list = new FilteredList(names);
      this._current_index = current_index;
      this._on_selection_made = on_selection_made;

      this._hover_index = current_index;
      this._scroll_to_index = current_index;
      this._scroll_offset = this.GetWindowSize().y - _row_height * 2;
    }

    #endregion -- Initialization ------------------------------------------

    #region -- Helper Classes ---------------------------------------------

    /// <summary>
    ///   Stores a list of strings and can return a subset of that list that
    ///   matches a given filter string.
    /// </summary>
    class FilteredList {
      /// <summary> All posibile items in the list. </summary>
      readonly string[] _all_items;

      /// <summary> Create a new filtered list. </summary>
      /// <param name="items">All The items to filter.</param>
      public FilteredList(string[] items) {
        this._all_items = items;
        this.Entries = new List<Entry>();
        this.UpdateFilter("");
      }

      /// <summary> The current string filtering the list. </summary>
      public string Filter { get; private set; }

      /// <summary> All valid entries for the current filter. </summary>
      public List<Entry> Entries { get; }

      /// <summary> Total possible entries in the list. </summary>
      public int MaxLength { get { return this._all_items.Length; } }

      /// <summary>
      ///   Sets a new filter string and updates the Entries that match the
      ///   new filter if it has changed.
      /// </summary>
      /// <param name="filter">String to use to filter the list.</param>
      /// <returns>
      ///   True if the filter is updated, false if newFilter is the same
      ///   as the current Filter and no update is necessary.
      /// </returns>
      public bool UpdateFilter(string filter) {
        if (this.Filter == filter) {
          return false;
        }

        this.Filter = filter;
        this.Entries.Clear();

        for (var i = 0; i < this._all_items.Length; i++) {
          if (string.IsNullOrEmpty(this.Filter)
              || this._all_items[i].ToLower().Contains(this.Filter.ToLower())) {
            var entry = new Entry {_Index = i, _Text = this._all_items[i]};
            if (string.Equals(this._all_items[i], this.Filter, StringComparison.CurrentCultureIgnoreCase)) {
              this.Entries.Insert(0, entry);
            } else {
              this.Entries.Add(entry);
            }
          }
        }

        return true;
      }

      /// <summary>
      ///   An entry in the filtererd list, mapping the text to the
      ///   original index.
      /// </summary>
      public struct Entry {
        public int _Index;
        public string _Text;
      }
    }

    #endregion -- Helper Classes ------------------------------------------

    #region -- Constants --------------------------------------------------

    /// <summary> Height of each element in the popup list. </summary>
    const float _row_height = 16.0f;

    /// <summary> How far to indent list entries. </summary>
    const float _row_indent = 8.0f;

    /// <summary> Name to use for the text field for search. </summary>
    const string _search_control_name = "EnumSearchText";

    #endregion -- Constants -----------------------------------------------

    #region -- Static Functions -------------------------------------------

    /// <summary> Show a new SearchablePopup. </summary>
    /// <param name="activator_rect">
    ///   Rectangle of the button that triggered the popup.
    /// </param>
    /// <param name="options">List of strings to choose from.</param>
    /// <param name="current">
    ///   Index of the currently selected string.
    /// </param>
    /// <param name="on_selection_made">
    ///   Callback to trigger when a choice is made.
    /// </param>
    public static void Show(Rect activator_rect,
                            string[] options,
                            int current,
                            Action<int> on_selection_made) {
      var win = new SearchablePopup(options, current, on_selection_made);
      PopupWindow.Show(activator_rect, win);
    }

    /// <summary>
    ///   Force the focused window to redraw. This can be used to make the
    ///   popup more responsive to mouse movement.
    /// </summary>
    static void Repaint() {
      var window = EditorWindow.focusedWindow;
      if (window) {
        window.Repaint();
      }
    }

    /// <summary> Draw a generic box. </summary>
    /// <param name="rect">Where to draw.</param>
    /// <param name="tint">Color to tint the box.</param>
    static void DrawBox(Rect rect, Color tint) {
      var c = GUI.color;
      GUI.color = tint;
      GUI.Box(rect, "", _selection);
      GUI.color = c;
    }

    #endregion -- Static Functions ----------------------------------------

    #region -- Private Variables ------------------------------------------

    /// <summary> Callback to trigger when an item is selected. </summary>
    readonly Action<int> _on_selection_made;

    /// <summary>
    ///   Index of the item that was selected when the list was opened.
    /// </summary>
    readonly int _current_index;

    /// <summary>
    ///   Container for all available options that does the actual string
    ///   filtering of the content.
    /// </summary>
    readonly FilteredList _list;

    /// <summary> Scroll offset for the vertical scroll area. </summary>
    Vector2 _scroll;

    /// <summary>
    ///   Index of the item under the mouse or selected with the keyboard.
    /// </summary>
    int _hover_index;

    /// <summary>
    ///   An item index to scroll to on the next draw.
    /// </summary>
    int _scroll_to_index;

    /// <summary>
    ///   An offset to apply after scrolling to scrollToIndex. This can be
    ///   used to control if the selection appears at the top, bottom, or
    ///   center of the popup.
    /// </summary>
    float _scroll_offset;

    #endregion -- Private Variables ---------------------------------------

    #region -- GUI Styles -------------------------------------------------

    // GUIStyles implicitly cast from a string. This triggers a lookup into
    // the current skin which will be the editor skin and lets us get some
    // built-in styles.

    static GUIStyle _search_box = "ToolbarSeachTextField";
    static GUIStyle _cancel_button = "ToolbarSeachCancelButton";
    static GUIStyle _disabled_cancel_button = "ToolbarSeachCancelButtonEmpty";
    static GUIStyle _selection = "SelectionRect";

    #endregion -- GUI Styles ----------------------------------------------

    #region -- PopupWindowContent Overrides -------------------------------

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnOpen() {
      base.OnOpen();
      // Force a repaint every frame to be responsive to mouse hover.
      EditorApplication.update += Repaint;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnClose() {
      base.OnClose();
      EditorApplication.update -= Repaint;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Vector2 GetWindowSize() {
      return new Vector2(base.GetWindowSize().x,
                         Mathf.Min(600,
                                   this._list.MaxLength * _row_height + EditorStyles.toolbar.fixedHeight));
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="rect"></param>
    public override void OnGUI(Rect rect) {
      var search_rect = new Rect(0,
                                 0,
                                 rect.width,
                                 EditorStyles.toolbar.fixedHeight);
      var scroll_rect = Rect.MinMaxRect(0,
                                        search_rect.yMax,
                                        rect.xMax,
                                        rect.yMax);

      this.HandleKeyboard();
      this.DrawSearch(search_rect);
      this.DrawSelectionArea(scroll_rect);
    }

    #endregion -- PopupWindowContent Overrides ----------------------------

    #region -- GUI --------------------------------------------------------

    void DrawSearch(Rect rect) {
      if (Event.current.type == EventType.Repaint) {
        EditorStyles.toolbar.Draw(rect,
                                  false,
                                  false,
                                  false,
                                  false);
      }

      var search_rect = new Rect(rect);
      search_rect.xMin += 6;
      search_rect.xMax -= 6;
      search_rect.y += 2;
      search_rect.width -= _cancel_button.fixedWidth;

      GUI.FocusControl(_search_control_name);
      GUI.SetNextControlName(_search_control_name);
      var new_text = GUI.TextField(search_rect, this._list.Filter, _search_box);

      if (this._list.UpdateFilter(new_text)) {
        this._hover_index = 0;
        this._scroll = Vector2.zero;
      }

      search_rect.x = search_rect.xMax;
      search_rect.width = _cancel_button.fixedWidth;

      if (string.IsNullOrEmpty(this._list.Filter)) {
        GUI.Box(search_rect, GUIContent.none, _disabled_cancel_button);
      } else if (GUI.Button(search_rect, "x", _cancel_button)) {
        this._list.UpdateFilter("");
        this._scroll = Vector2.zero;
      }
    }

    void DrawSelectionArea(Rect scroll_rect) {
      var content_rect = new Rect(0,
                                  0,
                                  scroll_rect.width - GUI.skin.verticalScrollbar.fixedWidth,
                                  this._list.Entries.Count * _row_height);

      this._scroll = GUI.BeginScrollView(scroll_rect, this._scroll, content_rect);

      var row_rect = new Rect(0,
                              0,
                              scroll_rect.width,
                              _row_height);

      for (var i = 0; i < this._list.Entries.Count; i++) {
        if (this._scroll_to_index == i
            && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)) {
          var r = new Rect(row_rect);
          r.y += this._scroll_offset;
          GUI.ScrollTo(r);
          this._scroll_to_index = -1;
          this._scroll.x = 0;
        }

        if (row_rect.Contains(Event.current.mousePosition)) {
          if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.ScrollWheel) {
            this._hover_index = i;
          }

          if (Event.current.type == EventType.MouseDown) {
            this._on_selection_made(this._list.Entries[i]._Index);
            EditorWindow.focusedWindow.Close();
          }
        }

        this.DrawRow(row_rect, i);

        row_rect.y = row_rect.yMax;
      }

      GUI.EndScrollView();
    }

    void DrawRow(Rect row_rect, int i) {
      if (this._list.Entries[i]._Index == this._current_index) {
        DrawBox(row_rect, Color.cyan);
      } else if (i == this._hover_index) {
        DrawBox(row_rect, Color.white);
      }

      var label_rect = new Rect(row_rect);
      label_rect.xMin += _row_indent;

      GUI.Label(label_rect, this._list.Entries[i]._Text);
    }

    /// <summary>
    ///   Process keyboard input to navigate the choices or make a selection.
    /// </summary>
    void HandleKeyboard() {
      if (Event.current.type == EventType.KeyDown) {
        if (Event.current.keyCode == KeyCode.DownArrow) {
          this._hover_index = Mathf.Min(this._list.Entries.Count - 1, this._hover_index + 1);
          Event.current.Use();
          this._scroll_to_index = this._hover_index;
          this._scroll_offset = _row_height;
        }

        if (Event.current.keyCode == KeyCode.UpArrow) {
          this._hover_index = Mathf.Max(0, this._hover_index - 1);
          Event.current.Use();
          this._scroll_to_index = this._hover_index;
          this._scroll_offset = -_row_height;
        }

        if (Event.current.keyCode == KeyCode.Return) {
          if (this._hover_index >= 0 && this._hover_index < this._list.Entries.Count) {
            this._on_selection_made(this._list.Entries[this._hover_index]._Index);
            EditorWindow.focusedWindow.Close();
          }
        }

        if (Event.current.keyCode == KeyCode.Escape) {
          EditorWindow.focusedWindow.Close();
        }
      }
    }

    #endregion -- GUI -----------------------------------------------------
  }
}
#endif
