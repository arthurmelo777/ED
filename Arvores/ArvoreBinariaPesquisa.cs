using System;
using System.Collections;

class NodeNotFound : Exception {

}

class Node {
    private Node parent; // pai
    private Node leftChild; // filho esquerdo
    private Node rightChild; // filho direito
    private object key; // chave

    public Node (Node p, object k) {
        parent = p;
        leftChild = null;
        rightChild = null;
        key = k;
    }

    public void setParent (Node p) {
        parent = p;
    }

    public Node getParent () {
        return parent;
    }

    public void setLeftChild (Node l) {
        leftChild = l;
    }

    public Node getLeftChild () {
        return leftChild;
    }

    public void setRightChild (Node r) {
        rightChild = r;
    }

    public Node getRightChild () {
        return rightChild;
    }

    public void setKey (object k) {
        key = k;
    }

    public object getKey () {
        return key;
    }

    public int countChilds () {
        if (leftChild == null && rightChild == null) {
            return 0;
        }
        if ((leftChild != null && rightChild == null) || (leftChild == null && rightChild != null)) {
            return 1;
        }
        return 2;
    }

    public bool leftOrRight () {
        return (leftChild != null); // se true, left não é vazio, se false, right não é vazio
    }
}

class Comparator {
    public int compare (object k1, object k2) {
        if ((int) k1 < (int) k2) {
            return -1;
        }
        else if ((int) k1 > (int) k2) {
            return 1;
        }
        return 0;
    }
}

class BinarySearchTree {
    // atributos
    private Node root;
    private ArrayList els;
    private ArrayList nds;
    private int countSize;
    private Comparator comp;
    private object[,] matriz;
    int x, y;

    //metodos
    public BinarySearchTree (object kr) {
        root = new Node (null, kr);
        countSize = 1;
    }

    public void setComp (Comparator c) {
        comp = c;
    }

    public Comparator getComp () {
        return comp;
    }

    public void setRoot (Node p) {
        root = p;
    }

    public Node getRoot () {
        return root;
    }

    public bool isExternal (Node v) {
        if (v.countChilds() == 0) {
            return true;
        }
        return false;
    }

    public bool isInternal (Node v) {
        if (isExternal(v)) {
            return false;
        }
        return true;
    }

    public int height (Node node) { // "feito"
        if (isExternal(node)) {
            return 0;
        }
        else {
            int h = 0;
            int children_height;
            if (node.getLeftChild() != null) {
                children_height = height(node.getLeftChild());
                h = Math.Max(h, children_height);
            }
            if (node.getRightChild() != null) {
                children_height = height(node.getRightChild());
                h = Math.Max(h, children_height);
            }
            return h+1;
        }
    } 

    public int depth (Node node) { // "feito"
        if (node == root) {
            return 0;
        }
        return 1 + depth(node.getParent());
    }

    public int size () { // "feito"
        return countSize;
    } 

    public bool isEmpty () {  // "feito"
        if (root == null) {
            return true;
        }
        return false;
    }
    
    public Node search (Node node, object key) { // "feito"
        if (comp.compare(key, node.getKey()) == 0) {
            return node;
        }
        if (comp.compare(key, node.getKey()) == -1) { // key < no
            if (node.getLeftChild() != null) { // e possui filho esquerdo
                return search(node.getLeftChild(), key);
            }
            else {
                return node;
            }
        }
        else {
            if (node.getRightChild() != null) {
                return search(node.getRightChild(), key);
            }
            else {
                return node;
            }
        }
    }

    public Node include (object key) { // "feito"
        Node node = search(root, key);
        Node n = new Node(node, key);

        if (comp.compare(n.getKey(), node.getKey()) == -1) {
            node.setLeftChild(n);
        }

        else if (comp.compare(n.getKey(), node.getKey()) == 1) {
            node.setRightChild(n);
        }

        countSize++;

        return n;
    }

    public object remove (object key) { // "feito"
        int c;
        Node node = search(root, key);
        // no nao existente
        if (node == null) {
            throw new NodeNotFound();
        }

        countSize--;

        // apenas raiz
        if (node.getParent() == null) {
            root = null;
            return node.getKey();
        }
        // no folha
        if (node.countChilds() == 0) {
            c = comp.compare(node.getKey(), node.getParent().getKey());

            if (c == -1) {
                node.getParent().setLeftChild(null);
            }
            else if (c == 1) {
                node.getParent().setRightChild(null);
            }

            return node.getKey();
        }

        // no com um filho
        if (node.countChilds() == 1) {
            bool lor = node.leftOrRight();
            c = comp.compare(node.getKey(), node.getParent().getKey());

            if (!lor) { // se left child for vazio
                if (c == -1) { // e node removido for o node esquerdo
                    node.getParent().setLeftChild(node.getRightChild()); // insira o filho direito do node removido no filho esquerdo do pai do node removido
                }
                else if (c == 1) { // e node removido for o node direito
                    node.getParent().setRightChild(node.getRightChild()); // insira o filho direito do node removido no filho direito do pai do node removido
                }
            }
            else { // se right child for vazio
                if (c == -1) { // e node removido for o node esquerdo
                    node.getParent().setLeftChild(node.getLeftChild()); // insira o filho direito do node removido no filho esquerdo do pai do node removido
                }
                else if (c == 1) { // e node removido for o node direito
                    node.getParent().setRightChild(node.getLeftChild()); // insira o filho direito do node removido no filho direito do pai do node removido
                }
            }

            return node.getKey();

        }

        // no com dois filhos
        if (node.countChilds() == 2) {
            Node suc;
            if (node.getRightChild().getLeftChild() == null) {
                node.setKey(node.getRightChild().getKey());
                node.setRightChild(null);
            }
            else {
                suc = sucessor(node.getRightChild());
                node.setKey(suc.getKey());
                suc.getParent().setLeftChild(null);
            }

            return node.getKey();
        }

        return null;
    }

    private Node sucessor (Node node) { // "feito"
        if (node.getLeftChild() == null) {
            return node;
        } else {
            return sucessor(node.getLeftChild());
        }
    }
    
    public void inOrder (Node node) { // "feito"
        if (node != null) {
            inOrder(node.getLeftChild());
            Console.WriteLine(node.getKey());
            inOrder(node.getRightChild());
        }
    }

    public void preOrder (Node node) { // "feito"
        if (node != null) {
            Console.WriteLine(node.getKey());
            preOrder(node.getLeftChild());
            preOrder(node.getRightChild());
        }
    } 

    public void postOrder (Node node) { // "feito"
        if (node != null) {
            postOrder(node.getLeftChild());
            postOrder(node.getRightChild());
            Console.WriteLine(node.getKey());
        }
    }

    public IEnumerator nodes () { // "feito"
        nds = new ArrayList();
        orderNodes(root);
        return nds.GetEnumerator();
    }

    private void orderNodes (Node node) { // "feito"
        nds.Add(node);
        if (node.getLeftChild() != null) {
            orderNodes(node.getLeftChild());
        }
        if (node.getRightChild() != null) {
            orderNodes(node.getRightChild());
        }
    }

    public IEnumerator elements () { // "feito"
        els = new ArrayList();
        orderElements(root);
        return els.GetEnumerator();
    }

    private void orderElements (Node node) { // "feito"
        els.Add(node.getKey());
        if (node.getLeftChild() != null) {
            orderElements(node.getLeftChild());
        }
        if (node.getRightChild() != null) {
            orderElements(node.getRightChild());
        }
    }

    public void print () { // "a fazer"
        nds = new ArrayList();
        allElementsCount(root);
        object[,] matrix = new object[height(root)+1, nds.Count];
        Node n;
        for (int i = 0; i < nds.Count; i++) {
            n = (Node) nds[i];
            matrix[depth(n), i] = n.getKey(); // matrix[x, y], tal que x é i e y é a altura do nó
        }

        for (int i = 0; i < height(root)+1; i++) {
            for (int j = 0; j < nds.Count; j++) {
                Console.Write($"{matrix[i, j]} ");
            }
            Console.WriteLine();
        }
    }

    private void allElementsCount (Node node) {
        if (node != null) {
            allElementsCount(node.getLeftChild());
            nds.Add(node);
            allElementsCount(node.getRightChild());
        }
    }
}

class Program {
    public static void Main () {
        BinarySearchTree bst = new BinarySearchTree(5);
        Comparator c = new Comparator();
        Node n;

        // set comparator
        bst.setComp(c);

        // root
        Console.WriteLine(bst.getRoot().getKey()); // 5
        n = new Node(null, 6);
        bst.setRoot(n);
        Console.WriteLine(bst.getRoot().getKey()); // 6

        // externo e interno
        Console.WriteLine(bst.isInternal(bst.getRoot())); // False
        Console.WriteLine(bst.isExternal(bst.getRoot())); // True

        // search
        Console.WriteLine(bst.search(bst.getRoot(), bst.getRoot().getKey()).getKey());

        // include
        bst.include(5);
        //Console.WriteLine(bst.height(bst.getRoot()));

        // remove
        bst.remove(5);

        // height
        Console.WriteLine(bst.height(bst.getRoot()));

        // depth
        Console.WriteLine(bst.depth(bst.getRoot()));

        // size
        Console.WriteLine(bst.size());

        // isEmpty
        Console.WriteLine();

        // inOrder
        bst.inOrder(bst.getRoot());

        // preOrder
        bst.preOrder(bst.getRoot());

        // postOrder
        bst.postOrder(bst.getRoot());

        // print
        bst.print();
    }
}