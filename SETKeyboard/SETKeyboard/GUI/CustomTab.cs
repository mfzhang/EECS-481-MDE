﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SETKeyboard.GUI
{
    public class CustomTab
    {
        private MainWindow window;
        private String consoleText;
        private String name;
        public int width;
        public int height;
       
        private Dictionary<String, TabPhrase> phrases;
        public CustomTab(MainWindow window, String name_, double height, double width)
        {
            this.window = window;
            consoleText = window.getConsoleText();
            name = name_;
            this.width = (int)width;
            this.height = (int)height;
            phrases = new Dictionary<String, TabPhrase>();
            renderTab(0);
        }
        private void renderTab(int type)
        {
            renderPhrases(type);
            renderControl();
        }

        private void renderPhrases(int type)
        {
            phrases.Clear();
            List<String> phraseStrings;
            if (window.tabPhrases.ContainsKey(name))
            {
                phraseStrings = window.tabPhrases[name].ToList();
                phraseStrings.Sort();
            }
            else
            {
                phraseStrings = new List<String>();
                window.tabPhrases.Add(name, new HashSet<String>());
            }
            if (!window.ctab_grids.ContainsKey(name))
            {
                window.assignGrid(name);
            }
            window.ctab_grids[name].Children.Clear();

            const int strlen_to_width_conversion = 12;
            int total_width = (int)(width);
            int button_height = (int)(height / 4) - 10;
            int button_height_margin = (int)(height / 4);
            int width_so_far = 0;
            int row = 0;
            for (int i = 0; i < phraseStrings.Count(); ++i)
            {
                TabPhrase tb;
                int button_width = 50 + phraseStrings[i].Length * strlen_to_width_conversion;
                int margin_left = (i > 0) ? width_so_far + 50 + phraseStrings[i - 1].Length * strlen_to_width_conversion + 10 : 0;
                if (i > 0 && width_so_far + margin_left > total_width - 10)
                {
                    ++row;
                    margin_left = 0;
                }
                width_so_far = margin_left;
                tb = new TabPhrase(phraseStrings[i], button_width, button_height, margin_left, button_height_margin + row * button_height_margin, 0, 0);
                assignEventHandler(type, tb);
                phrases.Add(phraseStrings[i], tb);
                window.ctab_grids[name].Children.Add(tb);
            }
        }
        private void assignEventHandler(int type, TabPhrase tb)
        {
            if (type < 2)
                tb.Click += new RoutedEventHandler(UseClick);
            else
                tb.Click += new RoutedEventHandler(RemoveClick);
        }

        private void renderControl()
        {
            TabPhrase create, remove, normal, clear, backspace;
            int button_height = (int)(height/4)-10;
            int button_width = (width - 30) / 4;
            create = new TabPhrase("(+) Phrase", button_width, button_height, 0, 0, 0, 0);
            remove = new TabPhrase("(-) Phrases", button_width, button_height, button_width + 10, 0, 0, 0);
            normal = new TabPhrase("Use Phrases", button_width, button_height, 2*(button_width+10), 0, 0, 0);
            clear = new TabPhrase("Clear Console", button_width, button_height, 3 * (button_width + 10), 0, 0, 0);
            backspace = new TabPhrase("Backspace", button_width, button_height, 4 * (button_width + 10), 0, 0, 0);
            create.Click += new RoutedEventHandler(UseInsertClick);
            normal.Click += new RoutedEventHandler(UseNormalClick);
            remove.Click += new RoutedEventHandler(UseRemoveClick);
            clear.Click += new RoutedEventHandler(ClearConsoleClick);
            backspace.Click += new RoutedEventHandler(Backspace_Click);
            window.ctab_grids[name].Children.Add(create);
            window.ctab_grids[name].Children.Add(normal);
            window.ctab_grids[name].Children.Add(remove);
            window.ctab_grids[name].Children.Add(clear);
            window.ctab_grids[name].Children.Add(backspace);
            
        }
        private void UseNormalClick(object sender, RoutedEventArgs e)
        {
        
            renderTab(0);
        }

        private void ClearConsoleClick(object sender, RoutedEventArgs e)
        {
            window.setConsoleText("");
            renderTab(0);
        }
        
        private void UseInsertClick(object sender, RoutedEventArgs e)
        {
            
            consoleText = window.getConsoleText();
            if (consoleText.Length!=0 && !window.tabPhrases[name].Contains(consoleText))
                window.tabPhrases[name].Add(consoleText);
            renderTab(1);
        }
        private void UseRemoveClick(object sender, RoutedEventArgs e)
        {
  
            renderTab(2);
        }
        private void UseClick(object sender, RoutedEventArgs e)
        {
            consoleText = window.getConsoleText();
            TabPhrase chosenPhrase = (TabPhrase)sender;
            String phrase = chosenPhrase.Content.ToString();
            consoleText += " ";
            consoleText += phrase;
            window.setConsoleText(consoleText);
        }
        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            TabPhrase deletePhrase = (TabPhrase)sender;
            String phrase = deletePhrase.Content.ToString();
            window.tabPhrases[name].Remove(phrase);
            renderTab(2);
        }
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            consoleText = window.getConsoleText();
            if ((consoleText.Length == 1) || (consoleText.Length == 0))
            {
                consoleText = "";
            }
            else
            {
                //Removes last character of console text string
                consoleText = consoleText.Substring(0, consoleText.Length - 1);
            }
            window.setConsoleText(consoleText);
            //window.TabPanel.Items.Remove()
            //window.TabPanel.Items.Remove(window.dictionary["new tab 2"]);

        }

    }
}
