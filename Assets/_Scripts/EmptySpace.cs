using UnityEngine;

namespace _Scripts
{
    public class EmptySpace : MonoBehaviour
    {
        
        private Bounds bound = new Bounds();
        
        private void Start()
        {
            //this.bound = GetComponent<BoxCollider>().bounds;
            var r = this.transform.gameObject.AddComponent<Rigidbody>();
            r.useGravity = false;
        }
        void OnTriggerEnter(Collider collision) 
        { 
            if (collision.gameObject.tag == "EmptyArea")
            {
                var collider = collision.gameObject.GetComponent<BoxCollider>();
                var bounds = collider.bounds;
                bounds.Encapsulate(GetComponent<BoxCollider>().bounds);
                this.bound = bounds;
                //collision.gameObject.SetActive(false);
                //transform.gameObject.SetActive(false);
            }
        }
        
        void OnCollisionEnter(Collision collision)
        {    
            //Check for a match with the specific tag on any GameObject that collides with your GameObject
            if (collision.gameObject.tag == "EmptyArea")
            {
                var collider = collision.gameObject.GetComponent<BoxCollider>();
                var bounds = collider.bounds;
                bounds.Encapsulate(GetComponent<BoxCollider>().bounds);
                this.bound = bounds; 
                //collision.gameObject.SetActive(false);
                //transform.gameObject.SetActive(false);
            }
        }
        
        
        private void OnDrawGizmos()
        {
            if (gameObject.activeSelf)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(bound.center, bound.size);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(bound.center, bound.size);
            }

        }
        
    }
    

}