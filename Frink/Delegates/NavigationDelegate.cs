using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Frink.Delegates
{
    /**
     * 
     * \brief Delegate class handling application navigation
     * methods
     * 
     * \author  Ivan Gudelj
     * \date    07.12.2015.
     * \version 1.0
     * \copyright   
     *      This code and information is provided "as is" without warranty of
     *      any kind, either expressed or implied, including but not limited to
     *      the implied warranties of merchantability and/or fitness for a
     *      particular purpose.
     */
    class NavigationDelegate
    {
        #region CLASS PARAMETERS



        private DrawerLayout.DrawerLayout _Parent;
        private int _InitialChildSize;



        #endregion
        #region CLASS CONSTRUCT



        public NavigationDelegate(DrawerLayout.DrawerLayout parent)
        {
            this._Parent = parent;
            this._InitialChildSize = parent.Children.Count;
        }



        #endregion
        #region NAVIGATION METHODS



        /// <summary>
        ///     Overwrites the current level instance item, preventing the depth
        ///     of the navigation going any further, similar to what tabbed 
        ///     navigation behaviour should display.
        /// </summary>
        /// <param name="item">UserControl to be added to the stack</param>
        public void AddItemTabbed(UserControl item)
        {
            if (_Parent.Children.Count > this._InitialChildSize)
                removeChildLast();

            _Parent.Children.Add(item);

            /*
            Transition transitionItem = new Transition;
            
            _Parent.ChildrenTransitions.Add(transitionItem);
            */
        }



        /// <summary>
        ///     Adds items to the backstack
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <param name="multipleinstances">Allow multiple instances of the user control in the backstack. FALSE if not defined</param>
        public void AddItem(UserControl item, bool multipleinstances = false)
        {
            if (!multipleinstances && this.hasChild(item))
                removeChild(item);

            if (_Parent.Children.Count > this._InitialChildSize)
                _Parent.Children.ElementAt(_Parent.Children.Count - 1).Visibility = Visibility.Collapsed;

            _Parent.Children.Add(item);
        }



        #endregion
        #region HELPER METHODS



        /// <summary>
        ///     Iterates through the collection of children elements comparing their class names 
        ///     to see if the children with the same class name already exists in the backstack.
        /// </summary>
        /// <param name="child">UserControl that's beeing looked for</param>
        /// <returns>if the child exists in the stack or not</returns>
        private bool hasChild(UserControl child)
        {
            foreach (var item in _Parent.Children)
            {
                if (item.GetType().Name.Equals(child.GetType().Name)) { return true; }
            }

            return false;
        }


        /// <summary>
        ///     Removes a child from the stack
        /// </summary>
        /// <param name="child">Child to be removed</param>
        private void removeChild(UserControl child)
        {
            _Parent.Children.RemoveAt(childIndex(child));
            GC.Collect();
        }


        /// <summary>
        ///     Removes the last child of the backstack
        /// </summary>
        private void removeChildLast()
        {
            _Parent.Children.RemoveAt(_Parent.Children.Count - 1);
            GC.Collect();
        }


        /// <summary>
        ///     Get the index of the child element.
        /// </summary>
        /// <param name="child">child element being looked for in the iteration</param>
        /// <returns>index of the child element, or 0</returns>
        private int childIndex(UserControl child)
        {
            for (int i = 0; i < _Parent.Children.Count; i++)
            {
                if (_Parent.Children.ElementAt(i).GetType().Name.Equals(child.GetType().Name))
                {
                    return i;
                }
            }
            return 0;
        }



        #endregion
        #region BACK BUTTON



        /// <summary>
        ///     Event handler that hooks on the back button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BackPressedStack(object sender, BackPressedEventArgs e)
        {
            if (_Parent.IsDrawerOpen)
            {
                _Parent.CloseDrawer();
                e.Handled = true;
            }
            else
            {
                this.BackPressed();
                e.Handled = true;
            }
        }


        /// <summary>
        ///     Logick for iterating through the backstack, and
        ///     exiting the app finally
        /// </summary>
        public void BackPressed()
        {
            if (_Parent.Children.Count == this._InitialChildSize)
            {
                Application.Current.Exit();
            }
            else
            {
                removeChildLast();
                _Parent.Children.Last().Visibility = Visibility.Visible;
            }
        }



        /// <summary>
        ///     Invoked when back button is pressed. It handels the funcitonality 
        ///     of going back to a previous screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
#if DEBUG
            var backstack = frame.BackStack;
            foreach (var item in backstack)
            {
                Debug.WriteLine("[NavigationDelegate][BackPressed] Full name: {0}", 
                    item.SourcePageType.FullName);
                
            }
            //Debug.WriteLine("[NavigationDelegate][BackPressed] {0}", frame.ContentTemplate.ToString());
#endif
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                try
                {
                    frame.GoBack();
                    if (e != null)
                        e.Handled = true;
                }
                catch (System.NullReferenceException error)
                {
#if DEBUG
                    Debug.WriteLine("[NavigationDelegate][BackPressed] error {0}", error.Message);
#endif
                    return;
                }
            }
        }



        #endregion
    }
}
