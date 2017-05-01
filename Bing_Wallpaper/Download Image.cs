using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bing_Wallpaper
{
    class UniqueQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();
        private HashSet<T> alreadyAdded = new HashSet<T>();

        public virtual void Enqueue(T item)
        {
            if (alreadyAdded.Add(item))
            {
                queue.Enqueue(item);
            }
        }

        public int Count
        {
            get { return queue.Count; }
        }

        public virtual T Dequeue()
        {
            T item = queue.Dequeue();
            return item;
        }

        public virtual bool Contains(T search)
        {
            return queue.Contains(search);
        }
    }

    class DownloadImageManager
    {
        private String basePath;
        private UniqueQueue<ImageDetails> queue;
        private List<ImageDetails> finisheDetailses = new List<ImageDetails>();

        public DownloadImageManager(String BasePath)
        {
            basePath = BasePath;
            queue = new UniqueQueue<ImageDetails>();

            Thread chacheThread = new Thread(new ThreadStart(ProcessImages));
            chacheThread.IsBackground = true;
            chacheThread.Start();
        }

        public void DownloadImage(ImageDetails details)
        {
            queue.Enqueue(details);
            int count = 0;
            while (!finisheDetailses.Contains(details))
            {
                count++;
            }
            return;
        }

        private void ProcessImages()
        {
            while (true)
            {
                while (queue.Count > 0)
                {
                    ImageDetails details = queue.Dequeue();
                    String fullPath = basePath + details.ImageFilePath;
                    using (WebClient client = new WebClient())
                    {
                        if (!File.Exists(fullPath))
                        {
                            client.DownloadFile(details.ImageUri, fullPath);
                        }
                    }
                    finisheDetailses.Add(details);
                }
            }
        }
    }
}